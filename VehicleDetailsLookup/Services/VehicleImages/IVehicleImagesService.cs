using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleImages
{
    /// <summary>
    /// Service interface for retrieving vehicle images.
    /// </summary>
    public interface IVehicleImagesService
    {
        /// <summary>
        /// Asynchronously retrieves vehicle images and details for the specified registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing vehicle details and associated images.
        /// </returns>
        Task<VehicleModel> GetVehicleImagesAsync(string registrationNumber);
    }
}
