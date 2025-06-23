using VehicleDetailsLookup.Shared.Models.Image;

namespace VehicleDetailsLookup.Services.Vehicle.Images
{
    /// <summary>
    /// Defines a contract for a service that retrieves images associated with a specific vehicle.
    /// </summary>
    public interface IVehicleImageService
    {
        /// <summary>
        /// Asynchronously retrieves a collection of images for a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">
        /// The unique registration number identifying the vehicle whose images are to be retrieved.
        /// </param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation. 
        /// The result contains an <see cref="IEnumerable{IImageModel}"/> of <see cref="IImageModel"/> instances,
        /// or <c>null</c> if no images are found for the specified vehicle.
        /// </returns>
        ValueTask<IEnumerable<IImageModel>?> GetVehicleImagesAsync(string registrationNumber);
    }
}
