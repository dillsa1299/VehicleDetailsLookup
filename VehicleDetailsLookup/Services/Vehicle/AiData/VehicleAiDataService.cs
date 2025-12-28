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
using VehicleDetailsLookup.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Requests;

namespace VehicleDetailsLookup.Services.Vehicle.AiData
{
    public class VehicleAiDataService(
        IGeminiService geminiService,
        IAiDataRepository aiDataRepository,
        IVehicleDetailsService vehicleDetailsService,
        IVehicleMotService vehicleMotService,
        IApiDatabaseMapperService apiMapper,
        IDatabaseFrontendMapperService databaseMapper) : IVehicleAiDataService
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

        public async ValueTask<AiDataModel?> GetVehicleAiDataAsync(GetVehicleAiDataRequest request)
        {
            var registrationNumber = request.RegistrationNumber;
            var searchType = request.SearchType;
            var metaData = request.MetaData;

            // Check if the AI data is already stored in the database
            var dbAiData = await _aiDataRepository.GetAiDataAsync(registrationNumber, searchType, metaData);

            TimeSpan cacheDuration = searchType == AiType.MotHistorySummary
                ? TimeSpan.FromMinutes(15) // MOT history summary cache duration - more frequent updates due to new MOT tests
                : TimeSpan.FromDays(1);

            if (dbAiData != null && (dbAiData.Updated > DateTime.UtcNow.Subtract(cacheDuration)))
                // Return stored AI data if it is recent enough
                return _databaseMapper.MapAiData(dbAiData!);

            return searchType switch
            {
                AiType.MotHistorySummary =>
                    await GetVehicleAiDataInternalAsync<IEnumerable<MotTestModel>>(
                        registrationNumber,
                        searchType,
                        metaData,
                        dbAiData,
                        await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber)),

                AiType.MotTestSummary =>
                    await GetMotTestSummary(registrationNumber, metaData, dbAiData),

                AiType.MotPriceEstimate =>
                    await GetMotPriceEstimate(registrationNumber, metaData, dbAiData),

                _ =>
                    await GetVehicleAiDataInternalAsync<DetailsModel>(
                        registrationNumber,
                        searchType,
                        metaData,
                        dbAiData,
                        await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber))
            };
        }

        private async ValueTask<AiDataModel?> GetMotTestSummary(string registrationNumber, string? metaData, AiDataDbModel? existingData)
        {
            if (string.IsNullOrWhiteSpace(metaData))
                return null;

            var data = JsonSerializer.Deserialize<AiMotTestSummaryMetaDataModel>(metaData);

            var motTests = await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber);
            var test = motTests?.FirstOrDefault(t => t.TestNumber == data?.TestNumber);
            var vehicleDetails = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (test == null || vehicleDetails == null)
                return null;

            var additionalData = new MotTestSummaryDataModel
            {
                YearOfManufacture = vehicleDetails.YearOfManufacture,
                Make = vehicleDetails.Make,
                Model = vehicleDetails.Model,
                EngineCapacity = vehicleDetails.EngineCapacity,
                FuelType = vehicleDetails.FuelType,
                TestModel = test
            };

            return await GetVehicleAiDataInternalAsync<MotTestSummaryDataModel>(
                registrationNumber,
                AiType.MotTestSummary,
                metaData,
                existingData,
                additionalData);
        }

        private async ValueTask<AiDataModel?> GetMotPriceEstimate(string registrationNumber, string? metaData, AiDataDbModel? existingData)
        {
            if (string.IsNullOrWhiteSpace(metaData))
                return null;

            var data = JsonSerializer.Deserialize<AiMotPriceEstimateMetaDataModel>(metaData);

            var motTests = await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber);
            var test = motTests?.FirstOrDefault(t => t.TestNumber == data?.TestNumber);
            var vehicleDetails = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (test == null || vehicleDetails == null || data == null || data.DefectIds == null || !data.DefectIds.Any())
                return null;

            var selectedDefects = test.Defects
                .Where(d => data.DefectIds.Contains(d.Id))
                .ToList();

            var additionalData = new MotPriceEstimateDataModel
            {
                YearOfManufacture = vehicleDetails.YearOfManufacture,
                Make = vehicleDetails.Make,
                Model = vehicleDetails.Model,
                EngineCapacity = vehicleDetails.EngineCapacity,
                FuelType = vehicleDetails.FuelType,
                Mileage = $"{test.OdometerValue} {test.OdometerUnit}",
                Defects = selectedDefects.Select(d => d.Description)
            };

            return await GetVehicleAiDataInternalAsync<MotPriceEstimateDataModel>(
                registrationNumber,
                AiType.MotPriceEstimate,
                metaData,
                existingData,
                additionalData);
        }

        private async ValueTask<AiDataModel?> GetVehicleAiDataInternalAsync<TData>(
            string registrationNumber,
            AiType searchType,
            string metaData,
            AiDataDbModel? existingData,
            TData? additionalData) where TData : class
        {
            if (additionalData == null)
                return null;

            if (existingData != null && existingData.DataHash == DataHash.GenerateHash(additionalData))
            {
                // Underlying data hasn't changed, update the timestamp and return existing AI data
                await _aiDataRepository.UpsertAiDataAsync(existingData);
                return _databaseMapper.MapAiData(existingData);
            }

            var prompt = BuildPrompt(searchType, additionalData);
            var aiResponse = await _geminiService.GetGeminiResponseAsync(prompt);

            if (aiResponse == null)
                return null;

            var dbAiData = _apiMapper.MapAiData(registrationNumber, searchType, metaData, aiResponse, DataHash.GenerateHash(additionalData));
            await _aiDataRepository.UpsertAiDataAsync(dbAiData);
            return _databaseMapper.MapAiData(dbAiData);
        }

        private static string BuildPrompt<TData>(AiType searchType, TData additionalData)
        {
            var additionalDataJson = JsonSerializer.Serialize(additionalData);

            return searchType switch
            {
                AiType.Overview =>
                    @$"Return the vehicle overview using the exact Markdown structure and ordering defined below.
                    Deviation from this structure is not allowed.

                    The response MUST be valid Markdown. Plain text responses are not allowed.

                    Markdown structure (use this exactly):

                    [1-2 short sentences describing the vehicle type and positioning.]

                    **Performance:**
                    - **Engine:** [Engine size and cylinder count, e.g. “1.0-litre 3-cylinder petrol”]
                    - **Power Output:** [In bhp]
                    - **0–62mph:** …
                    - **Top Speed:** [In mph]
                    - **Fuel Economy:** [In mpg - Urban/Extra Urban/Combined]

                    **Estimated UK Pricing:**
                    - **New List Price:** …
                    - **Used Market Range:** …

                    Content rules:
                    • Ensure a blank line separates the description, performance, and pricing sections exactly as in the structure above.
                    • The engine line MUST include both engine size and cylinder count together. If hybrid or electric, specify accordingly.
                    • Do NOT include engine details anywhere else.
                    • Use realistic UK-specification figures for the given year/make/model.
                    • Do NOT discuss common issues, reliability, or MOT history.
                    • Do NOT add, remove, or rename any fields.
                    • Do NOT include any additional headings or commentary.

                    Data: {additionalDataJson}",

                AiType.CommonIssues =>
                    @$"Based on the details of the following UK-specification vehicle, list realistic and commonly reported issues an owner should be aware of.
                        • Focus on problems that typically arise in real-world ownership (mechanical, electrical, reliability, or maintenance).
                        • Tailor issues to the vehicle’s age, mileage range, engine, and drivetrain where relevant.
                        • Avoid generic or speculative issues.
                        • No introduction, title, or concluding text.
                    Data: {additionalDataJson}",

                AiType.MotHistorySummary =>
                    @$"Write a concise narrative overview (2–3 sentences) of this vehicle’s UK MOT history across all provided tests.
                        • Focus on overall patterns and trends over time (e.g. recurring issues, deterioration, or improvements).
                        • Highlight any consistent mechanical, safety, or maintenance-related themes.
                        • Avoid listing individual tests or advisories.
                        • Do NOT mention pass/fail outcomes or specific dates.
                        • No introductory or concluding sentences.
                    Data: {additionalDataJson}",

                AiType.MotTestSummary =>
                    @$"Write a very short narrative summary (maximum 2–3 sentences) of the most significant mechanical or safety-related concerns from this UK MOT test.
                        • Focus only on the most serious or potentially safety-critical issues.
                        • Combine related issues into higher-level observations.
                        • Do NOT list individual advisories or repeat their wording.
                        • Do NOT mention whether the vehicle passed or failed.
                        • Ignore minor wear-and-tear unless it contributes to a larger concern.
                    Data: {additionalDataJson}",

                AiType.MotPriceEstimate =>
                    @$"Return a MOT repair cost estimate using the exact Markdown structure and ordering defined below.
                    Deviation from this structure is not allowed.

                    The response MUST be valid Markdown. Plain text responses are not allowed.

                    Markdown structure (use this exactly):

                    **Itemised Repairs:**
                    - **[Short defect name]**
                      - Parts: £min–£max
                      - Labour: £min–£max
                    - **[Short defect name]**
                      - Parts: £min–£max
                      - Labour: £min–£max

                    **Potential Additional Costs:**
                    - **[Item]:** £min–£max

                    **Total Estimated Cost (Parts & Labour):**
                    £min–£max

                    Content rules:
                    • No explanations, commentary, assumptions, or reasoning.
                    • Do NOT repeat full MOT defect wording — use short, clear repair names.
                    • Use realistic UK independent garage pricing.
                    • Assume aftermarket parts unless OEM is explicitly required.
                    • Each repair MUST include both Parts and Labour lines.
                    • Potential Additional Costs must be made up of likely additional costs based on the required work. E.g. alignment for suspension work.
                    • The Total Estimated Cost MUST include all parts, labour, and additional costs.
                    • If no additional costs are applicable, do not include the section.
                    • Do NOT add, remove, rename, or reorder sections.
                    • Do NOT include vehicle summaries or defect descriptions.
                    • Prices must be ranges, not single values.
                    • Currency must be GBP (£) only.

                    Data: {additionalDataJson}",

                _ =>
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, "Invalid AI data search type."),
            };
        }
    }
}
