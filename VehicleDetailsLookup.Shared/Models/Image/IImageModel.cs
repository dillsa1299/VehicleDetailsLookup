namespace VehicleDetailsLookup.Shared.Models.Image
{
    /// <summary>
    /// Defines the structure for image data associated with a vehicle.
    /// </summary>
    public interface IImageModel
    {
        /// <summary>
        /// The position or order of the image in a collection.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// The title or description of the image.
        /// </summary>
        string? Title { get; set; }
        /// <summary>
        /// The URL where the image is stored or can be accessed.
        /// </summary>
        string? Url { get; set; }
    }
}
