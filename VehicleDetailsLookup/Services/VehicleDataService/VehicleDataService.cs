using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleDataService
{
    /// <summary>
    /// Provides in-memory caching and retrieval of vehicle data.
    /// </summary>
    public class VehicleDataService : IVehicleDataService
    {
        /// <summary>
        /// In-memory cache for storing vehicle data, keyed by registration number.
        /// </summary>
        private static readonly Dictionary<string, VehicleModel> _vehicleCache = [];

        /// <summary>
        /// Retrieves a vehicle by its registration number.
        /// </summary>
        /// <param name="registration">The registration number of the vehicle.</param>
        /// <returns>The <see cref="VehicleModel"/> if found; otherwise, a new <see cref="VehicleModel"/> instance.</returns>
        public VehicleModel GetVehicle(string registration)
        {
            _vehicleCache.TryGetValue(registration, out var vehicle);
            return vehicle ?? new VehicleModel();
        }

        /// <summary>
        /// Updates or adds a vehicle in the cache.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> to update or add.</param>
        public void UpdateVehicle(VehicleModel vehicle)
        {
            _vehicleCache[vehicle.RegistrationNumber] = vehicle;
        }
    }
}
