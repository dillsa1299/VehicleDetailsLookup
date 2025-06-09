using System.Text.Json;
using VehicleDetailsLookup.Models.Enums;
using VehicleDetailsLookup.Models.SearchResponses.ImageSearch;
using VehicleDetailsLookup.Services.AiSearchService;
using VehicleDetailsLookup.Services.VehicleDataService;
using VehicleDetailsLookup.Services.VehicleDetailsService;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleAiService
{
    public class VehicleAiService(IAiSearchService aiSearch, IVehicleDetailsService vehicleDetails, IVehicleMapperService mapper, IVehicleDataService data) : IVehicleAiService
    {
        private readonly IAiSearchService _aiSearch = aiSearch
            ?? throw new ArgumentNullException(nameof(aiSearch));
        private readonly IVehicleDetailsService _vehicleDetails = vehicleDetails
            ?? throw new ArgumentNullException(nameof(vehicleDetails));
        private readonly IVehicleMapperService _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IVehicleDataService _data = data
            ?? throw new ArgumentNullException(nameof(data));

        public async Task<VehicleModel> SearchAiAsync(string registrationNumber, VehicleAiType searchType)
        {
            // Check if the vehicle is already cached
            var vehicle = _data.GetVehicle(registrationNumber);

            if (String.IsNullOrEmpty(vehicle.RegistrationNumber))
            {
                // No vehicle found, first try to get details
                vehicle = await _vehicleDetails.GetVehicleDetailsAsync(registrationNumber);

                // Unable to find vehicle details
                if (String.IsNullOrEmpty(vehicle.RegistrationNumber))
                    return new VehicleModel();
            }

            // Check if the AI data is already cached
            var aiData = vehicle.AiData.FirstOrDefault(ai => ai.Type == searchType);

            // TODO: Should be 1 day for overview/common issues, 15 minutes for MOT history summary
            if (aiData != null && aiData.LastUpdated > DateTime.UtcNow.AddMinutes(-15))
                // Return cached AI data if it is recent enough
                return vehicle;

            string prompt;
            string carDetails = $"Year={vehicle.YearOfManufacture}, Make={vehicle.Make}, Model={vehicle.Model}, Fuel Type={vehicle.FuelType}, Engine Capacity={vehicle.EngineCapacity}";

            switch (searchType)
            {
                case VehicleAiType.Overview:
                    prompt = $"Provide a brief overview of the following UK specification vehicle searching for additional information such as performance and pricing. " +
                             $"Don't discuss common issues. Give me the information directly without any introductory sentences or titles: " + carDetails;
                    break;
                case VehicleAiType.CommonIssues:
                    prompt = $"List the common issues with the UK specification of the following vehicle with no introduction/title: " + carDetails;
                    break;
                case VehicleAiType.MotHistorySummary:
                    var motResults = JsonSerializer.Serialize(vehicle.MotTests) ?? string.Empty;
                    prompt = $"Provide an overall summary for the following UK MOT test results. Exclude any introductory sentences: " + motResults;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }

            var aiResponse = await _aiSearch.SearchAiAsync(prompt);

            if (aiResponse == null || string.IsNullOrEmpty(aiResponse.Response))
                // If no AI response, return the vehicle without updating
                return new VehicleModel();

            vehicle = _mapper.MapAI(vehicle, aiResponse, searchType);

            _data.UpdateVehicle(vehicle);

            return vehicle;
        }
    }
}
