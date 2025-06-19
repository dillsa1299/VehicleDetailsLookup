namespace VehicleDetailsLookup.Models.Database.Image
{
    /// <summary>
    /// Defines the structure for storing vehicle image information in the database.
    /// </summary>
    public interface IImageDbModel
    {
        /// <summary>
        /// The vehicle's registration number associated with the image.
        /// </summary>
        string? RegistrationNumber { get; set; }
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