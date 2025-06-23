using System.Net.Http.Json;
using VehicleDetailsLookup.Shared.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Services.VehicleLookup
{
    public class VehicleLookupService(HttpClient httpClient) : IVehicleLookupService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async ValueTask<IDetailsModel?> GetVehicleDetailsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle details.
            var response = await _httpClient.GetAsync($"/api/VehicleDetails/{registrationNumber}");

            if (!response.IsSuccessStatusCode)
                // Return null if the request fails or vehicle not found.
                return null;

            var details = await response.Content.ReadFromJsonAsync<IDetailsModel>();

            return details;
        }

        public async ValueTask<IEnumerable<IMotTestModel>?> GetMotTestsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle MOT tests.
            var response = await _httpClient.GetAsync($"/api/VehicleMot/{registrationNumber}");

            // Return an empty list if the request fails or MOT tests not found.
            if (!response.IsSuccessStatusCode)
                return null;

            var motTests = await response.Content.ReadFromJsonAsync<IEnumerable<IMotTestModel>>();

            return motTests;
        }

        public async ValueTask<IEnumerable<IImageModel>?> GetVehicleImagesAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle images.
            var response = await _httpClient.GetAsync($"/api/VehicleImages/{registrationNumber}");

            // Return an empty VehicleModel if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return null;

            var images = await response.Content.ReadFromJsonAsync<IEnumerable<IImageModel>>();

            return images;
        }

        public async ValueTask<IAiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType type)
        {
            // Call the backend API to retrieve vehicle AI.
            var response = await _httpClient.GetAsync($"/api/VehicleAiData/{registrationNumber}/{type}");

            // Return an empty VehicleModel if the request fails or vehicle not found.
            if (!response.IsSuccessStatusCode)
                return null;

            var aiData = await response.Content.ReadFromJsonAsync<IAiDataModel>();

            return aiData;
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
