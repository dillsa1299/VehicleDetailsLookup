using VehicleDetailsLookup.Models.SearchResponses;

namespace VehicleDetailsLookup.Services.AiSearchService
{
    /// <summary>
    /// Defines a contract for AI-powered search services.
    /// </summary>
    public interface IAiSearchService
    {
        /// <summary>
        /// Performs an AI-based search using the specified prompt.
        /// </summary>
        /// <param name="prompt">The input prompt to send to the AI search service.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="AiSearchResponse"/>
        /// with the AI-generated response.
        /// </returns>
        Task<AiSearchResponse> SearchAiAsync(string prompt);
    }
}
