using VehicleDetailsLookup.Models.SearchResponses.ImageSearch;

namespace VehicleDetailsLookup.Services.ImageSearchService
{
    /// <summary>
    /// Provides an abstraction for searching images using a query string.
    /// Implementations of this interface should return relevant image search results
    /// based on the provided query.
    /// </summary>
    public interface IImageSearchService
    {
        /// <summary>
        /// Asynchronously searches for images that match the specified query string.
        /// </summary>
        /// <param name="query">The search query string used to find relevant images.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
        /// with a result of <see cref="ImageSearchResponse"/> containing the image search results.
        /// </returns>
        Task<ImageSearchResponse> SearchImagesAsync(string query);
    }
}
