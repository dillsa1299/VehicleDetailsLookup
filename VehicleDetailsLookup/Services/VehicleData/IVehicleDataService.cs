using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleData
{
    /// <summary>
    /// Defines methods for retrieving and updating vehicle data.
    /// </summary>
    public interface IVehicleDataService
    {
        /// <summary>
        /// Retrieves a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The <see cref="VehicleModel"/> corresponding to the registration number.</returns>
        VehicleModel GetVehicle(string registrationNumber);

        /// <summary>
        /// Updates the details of a vehicle.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> containing updated vehicle information.</param>
        void UpdateVehicle(VehicleModel vehicle);
    }
}
