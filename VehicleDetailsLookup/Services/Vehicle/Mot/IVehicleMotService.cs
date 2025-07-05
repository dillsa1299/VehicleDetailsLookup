using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Vehicle.Mot
{
    /// <summary>
    /// Service interface for retrieving MOT test details for a vehicle.
    /// </summary>
    public interface IVehicleMotService
    {
        /// <summary>
        /// Retrieves a collection of MOT test details for a vehicle based on its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of MOT test models
        /// or null if no data is available.
        /// </returns>
        ValueTask<IEnumerable<MotTestModel>?> GetVehicleMotTestsAsync(string registrationNumber);
    }
}
