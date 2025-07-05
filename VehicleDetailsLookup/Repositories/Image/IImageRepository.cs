using VehicleDetailsLookup.Models.Database.Image;

namespace VehicleDetailsLookup.Repositories.Image
{
    /// <summary>
    /// Defines methods for managing and retrieving vehicle image records in the database.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Adds new images or updates existing images for a vehicle in the database.
        /// </summary>
        /// <param name="images">A collection of image models to add or update. Each model must implement <see cref="IImageDbModel"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateImagesAsync(IEnumerable<ImageDbModel> images);

        /// <summary>
        /// Retrieves all images associated with a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle whose images are to be retrieved.</param>
        /// <returns>
        /// A value task containing a collection of image models implementing <see cref="IImageDbModel"/> for the specified vehicle,
        /// or <c>null</c> if no images exist for the given registration number.
        /// </returns>
        ValueTask<IEnumerable<ImageDbModel>?> GetImagesAsync(string registrationNumber);
    }
}
