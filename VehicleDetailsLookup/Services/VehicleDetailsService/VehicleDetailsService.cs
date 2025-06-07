using System.Text.Json;
using System.Text.RegularExpressions;
using VehicleDetailsLookup.Models.SearchResponses;
using VehicleDetailsLookup.Models.SearchResponses.MotSearch;
using VehicleDetailsLookup.Services.VehicleDataService;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleDetailsService
{
    public class VehicleDetailsService(HttpClient httpClient, IConfiguration configuration, IVehicleMapperService mapper, IVehicleDataService data) : IVehicleDetailsService
    {
        private readonly HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly IVehicleMapperService _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IVehicleDataService _data = data
            ?? throw new ArgumentNullException(nameof(data));

        private readonly string _vesKey = configuration["APIs:VES:Key"]
                      ?? throw new InvalidOperationException("VES API key not found in configuration.");
        private readonly string _vesURL = configuration["APIs:VES:URL"]
                      ?? throw new InvalidOperationException("VES API URL not found in configuration.");

        private readonly string _motUrl = configuration["APIs:MOT:URL"]
                      ?? throw new InvalidOperationException("MOT API URL not found in configuration.");
        private readonly string _motClientId = configuration["APIs:MOT:ClientId"]
                           ?? throw new InvalidOperationException("MOT API ClientId not found in configuration.");
        private readonly string _motClientSecret = configuration["APIs:MOT:ClientSecret"]
                               ?? throw new InvalidOperationException("MOT API ClientSecret not found in configuration.");
        private readonly string _motKey = configuration["APIs:MOT:Key"]
                      ?? throw new InvalidOperationException("MOT API key not found in configuration.");
        private readonly string _motScopeUrl = configuration["APIs:MOT:ScopeUrl"]
                           ?? throw new InvalidOperationException("MOT API ScopeUrl not found in configuration.");
        private readonly string _motTokenUrl = configuration["APIs:MOT:TokenUrl"]
                           ?? throw new InvalidOperationException("MOT API TokenUrl not found in configuration.");
        private MotAuthToken _motAuthToken = new();

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<VehicleModel> GetVehicleDetailsAsync(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber) || !Regex.IsMatch(registrationNumber, @"^[a-zA-Z0-9]{0,7}$"))
            {
                throw new ArgumentException("Registration number cannot be null or empty.", nameof(registrationNumber));
            }

            // Capitalize input
            registrationNumber = registrationNumber.ToUpperInvariant();

            // Check if the vehicle details are already cached
            var vehicle = _data.GetVehicle(registrationNumber);

            if (vehicle.DetailsLastUpdated > DateTime.UtcNow.AddMinutes(-15))
            {
                return vehicle; // Return stored vehicle details
            }

            var vesSearchResponse = await SearchVesAsync(registrationNumber);
            if (vesSearchResponse == null || String.IsNullOrEmpty(vesSearchResponse.RegistrationNumber)) return new VehicleModel();

            var motSearchResponse = await SearchMotAsync(registrationNumber);
            if (motSearchResponse == null || String.IsNullOrEmpty(motSearchResponse.Registration)) return new VehicleModel();

            vehicle = _mapper.MapDetails(vehicle, vesSearchResponse, motSearchResponse);

            _data.UpdateVehicle(vehicle);

            return vehicle;
        }

        private async Task<VesSearchResponse> SearchVesAsync(string registrationNumber)
        {
            // Build the request for the VES API
            using var request = new HttpRequestMessage(HttpMethod.Post, _vesURL)
            {
                Content = JsonContent.Create(new
                {
                    registrationNumber
                })
            };
            request.Headers.Add("x-api-key", _vesKey);

            // Send request
            using var response = await _httpClient.SendAsync(request);

            // Check if the response is successful
            if (!response.IsSuccessStatusCode)
                return new VesSearchResponse();

            // Parse response
            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<VesSearchResponse>(responseContent, _jsonSerializerOptions);

            return parsedResponse ?? new VesSearchResponse();
        }

        private async Task<MotSearchResponse> SearchMotAsync(string registrationNumber)
        {
            // Get authentication token
            if (!await GetMotTokenAsync()) return new MotSearchResponse();

            // Build request for the MOT API
            var motRequest = new HttpRequestMessage(HttpMethod.Get, $"{_motUrl}/{Uri.EscapeDataString(registrationNumber)}");
            motRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_motAuthToken.Type, _motAuthToken.Token);
            motRequest.Headers.Add("x-api-key", _motKey);

            // Send request
            using var motResponse = await _httpClient.SendAsync(motRequest);

            // Check if the response is successful
            if (!motResponse.IsSuccessStatusCode)
                return new MotSearchResponse();

            // Parse response
            var motContent = await motResponse.Content.ReadAsStringAsync();
            var parsedResponse = JsonSerializer.Deserialize<MotSearchResponse>(motContent, _jsonSerializerOptions);

            return parsedResponse ?? new MotSearchResponse();
        }

        private async Task<bool> GetMotTokenAsync()
        {
            // Check if current token is still valid
            if (_motAuthToken.ExpireTime > DateTime.UtcNow) return true;

            // Build request to get a new token
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _motTokenUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = _motClientId,
                    ["client_secret"] = _motClientSecret,
                    ["scope"] = _motScopeUrl,
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
            _motAuthToken = new MotAuthToken
            {
                Type = tokenType,
                ExpireTime = DateTime.UtcNow.AddSeconds(expiresIn),
                Token = accessToken
            };

            return true;
        }
    }
}
