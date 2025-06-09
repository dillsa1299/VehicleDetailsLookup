using System.Text;
using System.Text.Json;
using VehicleDetailsLookup.Models.SearchResponses;

namespace VehicleDetailsLookup.Services.AiSearchService
{
    public class AiSearchService(HttpClient httpClient, IConfiguration configuration) : IAiSearchService
    {
        private readonly HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly string _geminiUrl = configuration["APIs:Gemini:URL"]
                         ?? throw new InvalidOperationException("Gemini API URL not found in configuration.");
        private readonly string _geminiKey = configuration["APIs:Gemini:Key"]
                         ?? throw new InvalidOperationException("Gemini API key not found in configuration.");

        public async Task<AiSearchResponse> SearchAiAsync(string prompt)
        {
            var geminiRequest = new
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
            var jsonBody = JsonSerializer.Serialize(geminiRequest);
            var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            const int maxRetries = 10;
            int delayMs = 10;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                HttpResponseMessage response = await _httpClient.PostAsync(_geminiUrl + _geminiKey, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseString);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    if (string.IsNullOrEmpty(text))
                        return new AiSearchResponse();

                    return new AiSearchResponse { Response = text };
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
                    return new AiSearchResponse();
                }
            }

            // All retries failed, return empty response
            return new AiSearchResponse();
        }
    }
}
