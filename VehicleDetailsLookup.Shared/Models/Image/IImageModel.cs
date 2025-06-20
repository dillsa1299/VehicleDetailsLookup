namespace VehicleDetailsLookup.Shared.Models.Image
{
    /// <summary>
    /// Metadata for an image associated with a vehicle, including its order, title, and location.
    /// </summary>
    public interface IImageModel
    {
        /// <summary>
        /// Position or order of the image in a collection.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// Title or description of the image.
        /// </summary>
        string? Title { get; set; }
        /// <summary>
        /// URL where the image is stored or can be accessed.
        /// </summary>
        string? Url { get; set; }
    }
}
