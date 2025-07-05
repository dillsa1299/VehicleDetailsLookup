using System.Text.Json;
using VehicleDetailsLookup.Repositories.AiData;
using VehicleDetailsLookup.Services.Api.Gemini;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Services.Vehicle.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Vehicle.AiData
{
    public class VehicleAiDataService(IGeminiService geminiService, IAiDataRepository aiDataRepository, IVehicleDetailsService vehicleDetailsService, IVehicleMotService vehicleMotService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleAiDataService
    {
        private readonly IGeminiService _geminiService = geminiService
            ?? throw new ArgumentNullException(nameof(_geminiService));
        private readonly IAiDataRepository _aiDataRepository = aiDataRepository
            ?? throw new ArgumentNullException(nameof(_aiDataRepository));
        private readonly IVehicleDetailsService _vehicleDetailsService = vehicleDetailsService
            ?? throw new ArgumentNullException(nameof(_vehicleDetailsService));
        private readonly IVehicleMotService _vehicleMotService = vehicleMotService
            ?? throw new ArgumentNullException(nameof(_vehicleMotService));
        private readonly IApiDatabaseMapperService _apiMapper = apiMapper
            ?? throw new ArgumentNullException(nameof(apiMapper));
        private readonly IDatabaseFrontendMapperService _databaseMapper = databaseMapper
            ?? throw new ArgumentNullException(nameof(databaseMapper));

        public async ValueTask<AiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType searchType)
        {
            // Check if the vehicle details are already stored in the database
            var dbAiData = await _aiDataRepository.GetAiDataAsync(registrationNumber, searchType);
            
            TimeSpan cacheDuration = searchType == AiType.MotHistorySummary
                ? TimeSpan.FromMinutes(15) // MOT history summary cache duration
                : TimeSpan.FromDays(1); // Overview and common issues cache duration

            if (dbAiData?.Updated > DateTime.UtcNow.Subtract(cacheDuration))
                // Return stored AI data if it is recent enough
                return _databaseMapper.MapAiData(dbAiData);

            DetailsModel? vehicleDetails = null;
            IEnumerable<MotTestModel>? motTests = null;

            if (searchType == AiType.MotHistorySummary || searchType == AiType.ClarksonMotHistorySummary)
            {
                // For MOT history summary, we only need the MOT tests
                motTests = await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber);

                if (motTests == null)
                    // If no MOT tests found, return null
                    return null;
            }
            else
            {
                // Vehicle details are needed for overview and common issues
                vehicleDetails = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

                if (vehicleDetails == null)
                    // If no vehicle data is found, return null
                    return null;
            }

            var prompt = BuildPrompt(searchType, vehicleDetails, motTests);
            var geminiResponse = await _geminiService.GetGeminiResponseAsync(prompt);

            if (geminiResponse == null)
                // If no response from Gemini, return null
                return null;

            // Map the API response to database model and update the repository
            dbAiData = _apiMapper.MapAiData(registrationNumber, searchType, geminiResponse);
            await _aiDataRepository.UpdateAiDataAsync(dbAiData);

            return _databaseMapper.MapAiData(dbAiData);
        }

        private static string BuildPrompt(AiType searchType, DetailsModel? vehicleDetails, IEnumerable<MotTestModel>? motTests)
        {
            string data;
            switch (searchType)
            {
                case AiType.Overview:
                case AiType.CommonIssues:
                case AiType.ClarksonOverview:
                case AiType.ClarksonCommonIssues:
                    if (vehicleDetails == null)
                        throw new ArgumentNullException(nameof(vehicleDetails), "Vehicle details cannot be null for overview and common issues search types.");
                    data = JsonSerializer.Serialize(vehicleDetails);
                    break;
                case AiType.MotHistorySummary:
                case AiType.ClarksonMotHistorySummary:
                    if (motTests == null || !motTests.Any())
                        throw new ArgumentNullException(nameof(motTests), "MOT tests cannot be null or empty for MOT history summary search type.");
                    data = JsonSerializer.Serialize(motTests);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, "Invalid AI data search type.");
            }

            return searchType switch
            {
                AiType.Overview => $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {data}",
                AiType.CommonIssues => $"List the common issues with the UK specification of the following vehicle with no introduction/title: {data}",
                AiType.MotHistorySummary => $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {data}",
                AiType.ClarksonOverview => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {data}",
                AiType.ClarksonCommonIssues => $"Impersonate Jeremy Clarkson, voicing his opinions, and list the common issues with the UK specification of the following vehicle with no introduction/title: {data}",
                AiType.ClarksonMotHistorySummary => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {data}",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), searchType, "Invalid AI data search type."),
            };
        }
    }
}
