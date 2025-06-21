using System.Text.Json;
using VehicleDetailsLookup.Services.AiSearch;
using VehicleDetailsLookup.Services.VehicleData;
using VehicleDetailsLookup.Services.VehicleDetails;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Services.VehicleAi
{
    /// <summary>
    /// Provides AI-powered vehicle information lookup and caching logic.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VehicleAiService"/> class.
    /// </remarks>
    /// <param name="aiSearch">The AI search service for generating vehicle information.</param>
    /// <param name="vehicleDetails">The service for retrieving vehicle details.</param>
    /// <param name="mapper">The service for mapping AI responses to vehicle models.</param>
    /// <param name="data">The service for retrieving and updating vehicle data.</param>
    /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
    public class VehicleAiService(IAiSearchService aiSearch,
        IVehicleDetailsService vehicleDetails,
        IVehicleMapperService mapper,
        IVehicleDataService data) : IVehicleAiService
    {
        private readonly IAiSearchService _aiSearch = aiSearch
            ?? throw new ArgumentNullException(nameof(aiSearch));
        private readonly IVehicleDetailsService _vehicleDetails = vehicleDetails 
            ?? throw new ArgumentNullException(nameof(vehicleDetails));
        private readonly IVehicleMapperService _mapper = mapper 
            ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IVehicleDataService _data = data
            ?? throw new ArgumentNullException(nameof(data));

        /// <summary>
        /// Searches for AI-generated vehicle information based on the registration number and search type.
        /// Caches results for a configurable duration depending on the search type.
        /// </summary>
        /// <param name="registrationNumber">The vehicle registration number to search for.</param>
        /// <param name="searchType">The type of AI search to perform (e.g., Overview, CommonIssues, MotHistorySummary).</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the requested AI-generated information for the vehicle.
        /// </returns>
        public async Task<VehicleModel> SearchAiAsync(string registrationNumber, AiType searchType)
        {
            // Check if the vehicle is already cached
            var vehicle = _data.GetVehicle(registrationNumber);

            if (string.IsNullOrEmpty(vehicle.RegistrationNumber))
            {
                // No vehicle found, first try to get details
                vehicle = await _vehicleDetails.GetVehicleDetailsAsync(registrationNumber);

                // Unable to find vehicle details
                if (string.IsNullOrEmpty(vehicle.RegistrationNumber))
                    return new VehicleModel();
            }

            // Check if the AI data is already cached
            var aiData = vehicle.AiData.FirstOrDefault(ai => ai.Type == searchType);

            // Cache duration: 1 day for overview/common issues, 15 minutes for MOT history summary
            TimeSpan cacheDuration = searchType switch
            {
                AiType.Overview => TimeSpan.FromDays(1),
                AiType.CommonIssues => TimeSpan.FromDays(1),
                AiType.MotHistorySummary => TimeSpan.FromMinutes(15),
                _ => TimeSpan.FromMinutes(15)
            };

            if (aiData != null && aiData.LastUpdated > DateTime.UtcNow.Subtract(cacheDuration))
                // Return cached AI data if it is recent enough
                return vehicle;

            string prompt;
            string carDetails = $"Year={vehicle.YearOfManufacture}, Make={vehicle.Make}, Model={vehicle.Model}, Fuel Type={vehicle.FuelType}, Engine Capacity={vehicle.EngineCapacity}";

            switch (searchType)
            {
                case AiType.Overview:
                    prompt = $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                             $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: " + carDetails;
                    break;
                case AiType.CommonIssues:
                    prompt = $"List the common issues with the UK specification of the following vehicle with no introduction/title: " + carDetails;
                    break;
                case AiType.MotHistorySummary:
                    var motResults = JsonSerializer.Serialize(vehicle.MotTests) ?? string.Empty;
                    prompt = $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: " + motResults;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }

            var aiResponse = await _aiSearch.SearchAiAsync(prompt);

            if (aiResponse == null || string.IsNullOrEmpty(aiResponse.Response))
                // If no AI response, return the vehicle without updating
                return vehicle;

            vehicle = _mapper.MapAI(vehicle, aiResponse, searchType);

            _data.UpdateVehicle(vehicle);

            return vehicle;
        }
    }
}
