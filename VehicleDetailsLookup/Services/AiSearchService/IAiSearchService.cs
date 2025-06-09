using VehicleDetailsLookup.Models.SearchResponses;

namespace VehicleDetailsLookup.Services.AiSearchService
{
    public interface IAiSearchService
    {
        Task<AiSearchResponse> SearchImagesAsync(string query);
    }
}
