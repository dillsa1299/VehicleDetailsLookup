using System.Text.Json;
using VehicleDetailsLookup.Models.ApiResponses.Mot;

namespace VehicleDetailsLookup.Services.Api.Mot
{
    public class MotService(HttpClient httpClient, IConfiguration configuration) : IMotService
    {
        private readonly HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly string _url = configuration["APIs:MOT:URL"]
                      ?? throw new InvalidOperationException("MOT API URL not found in configuration.");
        private readonly string _clientId = configuration["APIs:MOT:ClientId"]
                           ?? throw new InvalidOperationException("MOT API ClientId not found in configuration.");
        private readonly string _clientSecret = configuration["APIs:MOT:ClientSecret"]
                               ?? throw new InvalidOperationException("MOT API ClientSecret not found in configuration.");
        private readonly string _apiKey = configuration["APIs:MOT:Key"]
                      ?? throw new InvalidOperationException("MOT API key not found in configuration.");
        private readonly string _scopeUrl = configuration["APIs:MOT:ScopeUrl"]
                           ?? throw new InvalidOperationException("MOT API ScopeUrl not found in configuration.");
        private readonly string _tokenUrl = configuration["APIs:MOT:TokenUrl"]
                           ?? throw new InvalidOperationException("MOT API TokenUrl not found in configuration.");
        private MotAuthToken _authToken = new();

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async ValueTask<IMotResponse?> GetMotResponseAsync(string registrationNumber)
        {
            // Get authentication token
            if (!await GetAuthTokenAsync())
                // Failed to get auth token
                return null;

            // Build request for the MOT API
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}/{Uri.EscapeDataString(registrationNumber)}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_authToken.Type, _authToken.Token);
            request.Headers.Add("x-api-key", _apiKey);

            // Send request
            using var response = await _httpClient.SendAsync(request);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
                return null;

            // Parse response
            var motContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<MotResponse>(motContent, _jsonSerializerOptions);

            return parsedResponse;
        }

        private async Task<bool> GetAuthTokenAsync()
        {
            // Check if current token is still valid
            if (_authToken.ExpireTime > DateTime.UtcNow)
                return true;

            // Build request to get a new token
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = _clientId,
                    ["client_secret"] = _clientSecret,
                    ["scope"] = _scopeUrl,
                    ["grant_type"] = "client_credentials"
                })
            };

            // Send request to get the token
            using var tokenResponse = await _httpClient.SendAsync(tokenRequest);

            // Check if token request was successful
            if (!tokenResponse.IsSuccessStatusCode)
                return false;

            // Parse token response
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            using var tokenDoc = JsonDocument.Parse(tokenContent);
            var tokenType = tokenDoc.RootElement.GetProperty("token_type").GetString();
            var expiresIn = tokenDoc.RootElement.GetProperty("expires_in").GetInt32();
            var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

            // Validate token details
            if (string.IsNullOrEmpty(tokenType) || string.IsNullOrEmpty(accessToken))
                return false;

            // Set new token details
            _authToken = new MotAuthToken
            {
                Type = tokenType,
                ExpireTime = DateTime.UtcNow.AddSeconds(expiresIn),
                Token = accessToken
            };

            return true;
        }
    }
}
