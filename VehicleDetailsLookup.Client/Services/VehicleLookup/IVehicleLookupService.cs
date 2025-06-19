using System.Net.Http;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;
using VehicleDetailsLookup.Shared.Models.VehicleLookup;

namespace VehicleDetailsLookup.Client.Services.VehicleLookup
{
    /// <summary>
    /// Provides methods for retrieving vehicle details, images, AI-generated information, and lookup statistics from the backend.
    /// </summary>
    public interface IVehicleLookupService
    {
        /// <summary>
        /// Retrieves detailed information about a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the vehicle's details.
        /// </returns>
        Task<VehicleModel> GetVehicleDetailsAsync(string registrationNumber);

        /// <summary>
        /// Retrieves image data associated with a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the vehicle's image data.
        /// </returns>
        Task<VehicleModel> GetVehicleImagesAsync(string registrationNumber);

        /// <summary>
        /// Retrieves AI-generated data for a vehicle, such as overviews or common issues, based on the specified AI data type.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <param name="type">The type of AI data to retrieve (e.g., overview, common issues).</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the requested AI-generated data.
        /// </returns>
        Task<VehicleModel> GetVehicleAIAsync(string registrationNumber, VehicleAiType type);

        /// <summary>
        /// Retrieves the number of times a vehicle has been looked up by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>
        /// An <see cref="int"/> representing the total lookup count for the specified vehicle.
        /// </returns>
        Task<int> GetVehicleLookupCountAsync(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of recent vehicle lookups.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{VehicleLookupModel}"/> containing details of recently looked up vehicles.
        /// </returns>
        Task<IEnumerable<VehicleLookupModel>> GetRecentVehicleLookupsAsync();
    }
}
