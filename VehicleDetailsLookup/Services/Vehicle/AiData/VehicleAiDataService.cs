using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Services.Api.Gemini;
using VehicleDetailsLookup.Services.Mappers;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Services.Vehicle.Mot;
using VehicleDetailsLookup.Shared.Models.AiData;
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

        public async ValueTask<IAiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType searchType)
        {
            // Check if the vehicle details are already stored in the database
            var dbAiData = _aiDataRepository.GetAiData(registrationNumber, searchType);
            
            TimeSpan cacheDuration = searchType == AiType.MotHistorySummary
                ? TimeSpan.FromMinutes(15) // MOT history summary cache duration
                : TimeSpan.FromDays(1); // Overview and common issues cache duration

            if (dbAiData?.Updated > DateTime.UtcNow.Subtract(cacheDuration))
                // Return stored AI data if it is recent enough
                return _databaseMapper.MapAiData(dbAiData);

            IDetailsModel? vehicleDetails = null;
            IEnumerable<IMotTestModel>? motTests = null;

            if (searchType == AiType.MotHistorySummary)
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
            _aiDataRepository.UpdateAiData(dbAiData);

            return _databaseMapper.MapAiData(dbAiData);
        }

        private static string BuildPrompt(AiType searchType, IDetailsModel? vehicleDetails, IEnumerable<IMotTestModel>? motTests)
        {
            if (vehicleDetails == null && searchType != AiType.MotHistorySummary)
                throw new ArgumentNullException(nameof(vehicleDetails), "Vehicle details cannot be null for overview and common issues search types.");

            string carDetails = string.Empty;
            if (searchType != AiType.MotHistorySummary && vehicleDetails != null)
            {
                carDetails = $"Year={vehicleDetails.YearOfManufacture}, Make={vehicleDetails.Make}, Model={vehicleDetails.Model}, Fuel Type={vehicleDetails.FuelType}, Engine Capacity={vehicleDetails.EngineCapacity}";
            }

            switch (searchType)
            {
                case AiType.Overview:
                    return $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {carDetails}";
                case AiType.CommonIssues:
                    return $"List the common issues with the UK specification of the following vehicle with no introduction/title: {carDetails}";
                case AiType.MotHistorySummary:
                    return $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {motTests}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
        }
    }
}
