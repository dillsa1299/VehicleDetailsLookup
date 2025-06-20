using VehicleDetailsLookup.Models.Database.Image;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving vehicle image records from the database.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Adds or updates images for a vehicle in the database.
        /// </summary>
        /// <param name="images">A collection of image models to add or update.</param>
        void UpdateImages(IEnumerable<ImageDbModel> images);

        /// <summary>
        /// Retrieves all images associated with a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve images for.</param>
        /// <returns>A collection of image models for the specified vehicle implementing <see cref="IImageDbModel"/>, or null if none exist.</returns>
        IEnumerable<IImageDbModel>? GetImages(string registrationNumber);
    }
}
