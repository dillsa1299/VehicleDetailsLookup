namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    /// <summary>
    /// Represents a single image result from a Google Image search response.
    /// </summary>
    public interface IGoogleImageResponseItem
    {
        /// <summary>
        /// URL of the image.
        /// </summary>
        string? Link { get; set; }

        /// <summary>
        /// Title or description of the image.
        /// </summary>
        string? Title { get; set; }
    }
}