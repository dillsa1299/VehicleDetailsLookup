using System.Net.Http.Json;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Services.VehicleLookup
{
    public class VehicleLookupService(HttpClient httpClient) : IVehicleLookupService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<VehicleModel> GetVehicleDetailsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle details.
            var response = await _httpClient.GetAsync($"/api/VehicleDetails/{registrationNumber}");

            // Return an empty VehicleModel if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return new VehicleModel();

            var vehicle = await response.Content.ReadFromJsonAsync<VehicleModel>();

            return vehicle is null ? throw new InvalidOperationException("Failed to deserialize vehicle data.") : vehicle;
        }

        public async Task<VehicleModel> GetVehicleImagesAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle images.
            var response = await _httpClient.GetAsync($"/api/VehicleImages/{registrationNumber}");

            // Return an empty VehicleModel if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return new VehicleModel();

            var vehicle = await response.Content.ReadFromJsonAsync<VehicleModel>();

            return vehicle is null ? throw new InvalidOperationException("Failed to deserialize vehicle data.") : vehicle;
        }

        public async Task<VehicleModel> GetVehicleAIAsync(string registrationNumber, VehicleAiType type)
        {
            // Call the backend API to retrieve vehicle AI.
            var response = await _httpClient.GetAsync($"/api/VehicleAi/{type}/{registrationNumber}");

            // Return an empty VehicleModel if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return new VehicleModel();

            var vehicle = await response.Content.ReadFromJsonAsync<VehicleModel>();

            return vehicle is null ? throw new InvalidOperationException("Failed to deserialize vehicle data.") : vehicle;
        }

        public async Task<int> GetVehicleLookupCountAsync(string registrationNumber)
        {
            // Call the backend API to retrieve the vehicle lookup count.
            var response = await _httpClient.GetAsync($"/api/VehicleLookupHistory/count/{registrationNumber}");

            // Return 0 if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return 0;

            var count = await response.Content.ReadFromJsonAsync<int>();

            return count;
        }

        public async Task<IEnumerable<LookupModel>> GetRecentVehicleLookupsAsync()
        {
            // Call the backend API to retrieve recent vehicle lookups.
            var response = await _httpClient.GetAsync("/api/VehicleLookupHistory/recent");

            // Return an empty list if the request fails or no recent vehicles found.
            if (!response.IsSuccessStatusCode)
                return [];

            var vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<LookupModel>>();

            return vehicles ?? throw new InvalidOperationException("Failed to deserialize recent vehicles data.");
        }
    }
}
