using System.Text.Json;
using VehicleDetailsLookup.Models.ApiResponses.Ves;

namespace VehicleDetailsLookup.Services.Api.Ves
{
    public class VesService(HttpClient httpClient, IConfiguration configuration) : IVesService
    {
        private readonly HttpClient _httpClient = httpClient
           ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly string _apiKey = configuration["APIs:VES:Key"]
                      ?? throw new InvalidOperationException("VES API key not found in configuration.");
        private readonly string _url = configuration["APIs:VES:URL"]
                      ?? throw new InvalidOperationException("VES API URL not found in configuration.");

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async ValueTask<IVesResponse?> GetVesResponseAsync(string registrationNumber)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = JsonContent.Create(new
                {
                    registrationNumber
                })
            };
            request.Headers.Add("x-api-key", _apiKey);

            using var response = await _httpClient.SendAsync(request);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
                return null;

            // Parse response
            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<VesResponse>(responseContent, _jsonSerializerOptions);

            return parsedResponse;
        }
    }
}
