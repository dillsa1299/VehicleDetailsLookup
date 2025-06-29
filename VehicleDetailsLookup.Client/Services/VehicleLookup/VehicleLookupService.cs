using Microsoft.JSInterop;
using System.Net.Http.Json;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Client.Services.VehicleLookup
{
    public class VehicleLookupService(HttpClient httpClient, IJSRuntime jsRuntime) : IVehicleLookupService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async ValueTask<IDetailsModel?> GetVehicleDetailsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle details.
            var response = await _httpClient.GetAsync($"/api/VehicleDetails/{registrationNumber}");

            if (!response.IsSuccessStatusCode)
                // Return null if the request fails or vehicle not found.
                return null;

            var details = await response.Content.ReadFromJsonAsync<DetailsModel>();

            return details;
        }

        public async ValueTask<IEnumerable<IMotTestModel>?> GetMotTestsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle MOT tests.
            var response = await _httpClient.GetAsync($"/api/VehicleMot/{registrationNumber}");

            // Return null if the request fails or no MOT tests found.
            if (!response.IsSuccessStatusCode)
                return null;

            var motTests = await response.Content.ReadFromJsonAsync<IEnumerable<MotTestModel>>();

            return motTests?.OrderByDescending(t => t.CompletedDate);
        }

        public async ValueTask<IEnumerable<IImageModel>?> GetVehicleImagesAsync(string registrationNumber)
        {
            // Call the backend API to retrieve vehicle images.
            var response = await _httpClient.GetAsync($"/api/VehicleImages/{registrationNumber}");

            // Return null if the request fails or no images found.
            if (!response.IsSuccessStatusCode)
                return null;

            var images = await response.Content.ReadFromJsonAsync<IEnumerable<ImageModel>>();

            if (images != null && images.Any())
            {
                // Try to load images in the browser to check if they are accessible.
                var loadedUrls = await _jsRuntime.InvokeAsync<IEnumerable<string>>("checkImagesLoad", images.Select(image => image.Url));

                // Filter out images that failed to load.
                images = images.Where(image => loadedUrls.Contains(image.Url));

                // Update the index of each image in-place after filtering.
                int index = 1;
                foreach (var image in images)
                    image.Index = index++;
            }

            return images;
        }

        public async ValueTask<IAiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType type)
        {
            // Call the backend API to retrieve vehicle AI.
            var response = await _httpClient.GetAsync($"/api/VehicleAiData/{registrationNumber}/{type}");

            // Return null if the request fails or AI data not found.
            if (!response.IsSuccessStatusCode)
                return null;

            var aiData = await response.Content.ReadFromJsonAsync<AiDataModel>();

            return aiData;
        }

        public async ValueTask<int?> GetVehicleLookupCountAsync(string registrationNumber)
        {
            // Call the backend API to retrieve the vehicle lookup count.
            var response = await _httpClient.GetAsync($"/api/VehicleLookupHistory/count/{registrationNumber}");

            // Return null if the request fails or no lookup history found.
            if (!response.IsSuccessStatusCode)
                return null;

            var count = await response.Content.ReadFromJsonAsync<int>();

            return count;
        }

        public async ValueTask<IEnumerable<ILookupModel>?> GetRecentVehicleLookupsAsync()
        {
            // Call the backend API to retrieve recent vehicle lookups.
            var response = await _httpClient.GetAsync("/api/VehicleLookupHistory/recent");

            // Return null if the request fails or no recent lookups found.
            if (!response.IsSuccessStatusCode)
                return null;

            var lookups = await response.Content.ReadFromJsonAsync<IEnumerable<LookupModel>>();

            return lookups;
        }

        public async ValueTask<IEnumerable<ILookupModel>?> GetRecentVehicleLookupsAsync(string registrationNumber)
        {
            // Call the backend API to retrieve recent lookups for the registration number.
            var response = await _httpClient.GetAsync($"/api/VehicleLookupHistory/recent/{registrationNumber}");

            // Return null if the request fails or no lookup history found.
            if (!response.IsSuccessStatusCode)
                return null;

            var lookups = await response.Content.ReadFromJsonAsync<IEnumerable<LookupModel>>();

            return lookups;
        }
    }
}
