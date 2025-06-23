using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;

namespace VehicleDetailsLookup.Services.Api.GoogleImage
{
    public class GoogleImageService(HttpClient httpClient, IConfiguration configuration) : IGoogleImageService
    {
        private readonly HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly string _googleKey = configuration["APIs:Google:Key"]
            ?? throw new InvalidOperationException("Google API key not found in configuration.");
        private readonly string _googleCx = configuration["APIs:Google:Cx"]
            ?? throw new InvalidOperationException("Google API cx not found in configuration.");

        public async ValueTask<IGoogleImageResponse?> GetGoogleImageResponseAsync(string query)
        {
            // TODO: Revisit this to allow passing in additional parameters
            var url = $"https://www.googleapis.com/customsearch/v1?q={Uri.EscapeDataString(query)}&cx={_googleCx}&key={_googleKey}&searchType=image";

            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
                return null;

            var response = await httpResponse.Content.ReadFromJsonAsync<GoogleImageResponse>();

            return response;
        }
    }
}
