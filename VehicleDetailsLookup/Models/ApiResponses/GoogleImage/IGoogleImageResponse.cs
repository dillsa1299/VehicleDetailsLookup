
namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    /// <summary>
    /// Represents a response from the Google Image API.
    /// </summary>
    public interface IGoogleImageResponse
    {
        /// <summary>
        /// Collection of image items.
        /// </summary>
        IEnumerable<GoogleImageResponseItem>? Items { get; set; }
    }
}