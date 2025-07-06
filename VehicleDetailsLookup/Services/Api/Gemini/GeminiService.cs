using System.Text;
using System.Text.Json;
using VehicleDetailsLookup.Models.ApiResponses.Gemini;
using VehicleDetailsLookup.Models.ApiResponses.Mot;

namespace VehicleDetailsLookup.Services.Api.Gemini
{
    public class GeminiService(HttpClient httpClient, IConfiguration configuration) : IGeminiService
    {
        private readonly HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly string _url = configuration["APIs:Gemini:URL"]
                         ?? throw new InvalidOperationException("Gemini API URL not found in configuration.");
        private readonly string _key = configuration["APIs:Gemini:Key"]
                         ?? throw new InvalidOperationException("Gemini API key not found in configuration.");

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async ValueTask<GeminiResponseModel?> GetGeminiResponseAsync(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };
            var jsonBody = JsonSerializer.Serialize(requestBody);
            var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            const int maxRetries = 10;
            int delayMs = 10;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _url + _key)
                {
                    Content = requestContent
                };

                using var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var parsedResponse = await JsonSerializer.DeserializeAsync<GeminiResponseModel>(responseStream, _jsonSerializerOptions);
                    return parsedResponse;
                }
                else if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600 && attempt < maxRetries)
                {
                    // Exponential backoff
                    await Task.Delay(delayMs);
                    delayMs *= 2;
                    continue;
                }
                else
                {
                    // Bad request or other error, return empty response
                    return null;
                }
            }

            // All retries failed, return empty response
            return null;
        }
    }
}
