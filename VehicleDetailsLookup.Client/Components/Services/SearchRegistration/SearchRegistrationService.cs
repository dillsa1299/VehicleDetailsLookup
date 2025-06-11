using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using VehicleDetailsLookup.Client.Components.Services.SearchRegistration;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleInformationChecker.Components.Services.SearchRegistration
{
    public sealed class SearchRegistrationService(HttpClient httpClient) : ISearchRegistrationService
    {
        private readonly HttpClient _httpClient = httpClient;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<VehicleModel> SearchVehicleAsync(VehicleModel vehicle, SearchType searchType)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

    }
}
