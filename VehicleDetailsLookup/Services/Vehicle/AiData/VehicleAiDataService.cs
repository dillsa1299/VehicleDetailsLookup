using System.Text.Json;
using VehicleDetailsLookup.Client.Components.UI.VehicleDetails;
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

            return searchType switch
            {
                AiType.MotHistorySummary or AiType.ClarksonMotHistorySummary => await GetVehicleAiDataInternalAsync<IEnumerable<MotTestModel>>(registrationNumber, searchType, _vehicleMotService.GetVehicleMotTestsAsync),
                _ => await GetVehicleAiDataInternalAsync<DetailsModel>(registrationNumber, searchType, (reg) => _vehicleDetailsService.GetVehicleDetailsAsync(reg, false))
            };
        }

        private async ValueTask<AiDataModel?> GetVehicleAiDataInternalAsync<TData>(string registrationNumber, AiType searchType, Func<string, ValueTask<TData?>> getDataAsync) where TData : class
        {
            var data = await getDataAsync(registrationNumber);
            if (data == null)
                // Failed to retrieve data, return null
                return null;

            var prompt = BuildPrompt(searchType, data);
            var geminiResponse = await _geminiService.GetGeminiResponseAsync(prompt);

            if (geminiResponse == null)
                // Failed to get a response from the Gemini API, return null
                return null;

            var dbAiData = _apiMapper.MapAiData(registrationNumber, searchType, geminiResponse);
            await _aiDataRepository.UpdateAiDataAsync(dbAiData);
            return _databaseMapper.MapAiData(dbAiData);
        }

        private static string BuildPrompt<TData>(AiType searchType, TData data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            return searchType switch
            {
                AiType.Overview => $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {dataJson}",
                AiType.CommonIssues => $"List the common issues with the UK specification of the following vehicle with no introduction/title: {dataJson}",
                AiType.MotHistorySummary => $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {dataJson}",
                AiType.ClarksonOverview => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {dataJson}",
                AiType.ClarksonCommonIssues => $"Impersonate Jeremy Clarkson, voicing his opinions, and list the common issues with the UK specification of the following vehicle with no introduction/title: {dataJson}",
                AiType.ClarksonMotHistorySummary => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {dataJson}",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), searchType, "Invalid AI data search type."),
            };
        }
    }
}
