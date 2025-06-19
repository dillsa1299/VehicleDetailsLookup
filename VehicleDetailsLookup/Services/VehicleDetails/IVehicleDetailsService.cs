using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Services.VehicleDetails
{
    /// <summary>
    /// Provides an abstraction for retrieving detailed information about vehicles.
    /// </summary>
    public interface IVehicleDetailsService
    {
        /// <summary>
        /// Asynchronously retrieves the details of a vehicle using its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the vehicle's details if found; otherwise, an empty <see cref="VehicleModel"/>.
        /// </returns>
        Task<VehicleModel> GetVehicleDetailsAsync(string registrationNumber);
    }
}
