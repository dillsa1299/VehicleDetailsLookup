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
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Models.Database.AiData;

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
                AiType.MotHistorySummary or AiType.ClarksonMotHistorySummary => await GetVehicleAiDataInternalAsync<IEnumerable<MotTestModel>>(registrationNumber, searchType, dbAiData, await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber)),
                _ => await GetVehicleAiDataInternalAsync<DetailsModel>(registrationNumber, searchType, dbAiData, await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber, false))
            };
        }

        private async ValueTask<AiDataModel?> GetVehicleAiDataInternalAsync<TData>(string registrationNumber, AiType searchType, AiDataDbModel? existingData,TData? additionalData) where TData : class
        {
            if (additionalData == null)
                return null;

            if (existingData != null && existingData.DataHash == DataHash.GenerateHash(additionalData))
            {
                // Underlying data hasn't changed, update the timestamp and return existing AI data
                await _aiDataRepository.UpdateAiDataAsync(existingData);
                return _databaseMapper.MapAiData(existingData);
            }

            var prompt = BuildPrompt(searchType, additionalData);
            var aiResponse = await _geminiService.GetGeminiResponseAsync(prompt);

            if (aiResponse == null)
                return null;

            var dbAiData = _apiMapper.MapAiData(registrationNumber, searchType, aiResponse, DataHash.GenerateHash(additionalData));
            await _aiDataRepository.UpdateAiDataAsync(dbAiData);
            return _databaseMapper.MapAiData(dbAiData);
        }

        private static string BuildPrompt<TData>(AiType searchType, TData additionalData)
        {
            var additionalDataJson = JsonSerializer.Serialize(additionalData);
            return searchType switch
            {
                AiType.Overview => $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {additionalDataJson}",
                AiType.CommonIssues => $"List the common issues with the UK specification of the following vehicle with no introduction/title: {additionalDataJson}",
                AiType.MotHistorySummary => $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {additionalDataJson}",
                AiType.ClarksonOverview => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                                           $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: {additionalDataJson}",
                AiType.ClarksonCommonIssues => $"Impersonate Jeremy Clarkson, voicing his opinions, and list the common issues with the UK specification of the following vehicle with no introduction/title: {additionalDataJson}",
                AiType.ClarksonMotHistorySummary => $"Impersonate Jeremy Clarkson, voicing his opinions, and provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: {additionalDataJson}",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), searchType, "Invalid AI data search type."),
            };
        }
    }
}
