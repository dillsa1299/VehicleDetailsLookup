using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;

namespace VehicleDetailsLookup.Services.Api.GoogleImage
{
    /// <summary>
    /// Provides an abstraction for searching images using a query string.
    /// Implementations of this interface should return relevant image search results
    /// based on the provided query.
    /// </summary>
    public interface IGoogleImageService
    {
        /// <summary>
        /// Asynchronously searches for images that match the specified query string.
        /// </summary>
        /// <param name="query">The search query string used to find relevant images.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation,
        /// with a result of <see cref="IGoogleImageResponse"/> containing the image search results,
        /// or <c>null</c> if no results are found or an error occurs.
        /// </returns>
        ValueTask<GoogleImageResponseModel?> GetGoogleImageResponseAsync(string query);
    }
}
