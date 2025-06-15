using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleData
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
        /// In-memory cache for storing when vehicle lookups were last performed, keyed by registration number.
        /// </summary>
        private static readonly Dictionary<DateTime, VehicleLookupModel> _vehicleLookupCache = [];

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

        /// <summary>
        /// Logs a vehicle lookup by registration number, recording the current UTC time.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle being looked up.</param>
        public void LogLookup(string registrationNumber)
        {
            var vehicleLookupModel = new VehicleLookupModel
            {
                DateTime = DateTime.UtcNow,
                RegistrationNumber = registrationNumber
            };
            // Update the last lookup time for the vehicle
            _vehicleLookupCache[vehicleLookupModel.DateTime] = vehicleLookupModel;
        }

        /// <summary>
        /// Gets the number of times a vehicle has been looked up by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The number of lookups for the specified registration number.</returns>
        public int GetVehicleLookupCount(string registrationNumber)
        {
            return _vehicleLookupCache.Count(lookup => lookup.Value.RegistrationNumber == registrationNumber);
        }

        /// <summary>
        /// Retrieves a collection of recent vehicle lookups.
        /// </summary>
        /// <param name="count">The maximum number of recent lookups to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{VehicleModel}"/> containing the most recent vehicles
        /// that have been looked up, ordered by lookup date descending.
        /// </returns>
        public IEnumerable<VehicleModel> GetRecentLookups(int count)
        {
            // Get lookup values
            var lookups = _vehicleLookupCache.Values
                .OrderByDescending(lookup => lookup.DateTime)
                .Take(count);

            // Load corresponding vehicle data
            var vehicles = new List<VehicleModel>();
            foreach (var lookup in lookups)
            {
                if (_vehicleCache.TryGetValue(lookup.RegistrationNumber, out var vehicle))
                {
                    vehicles.Add(vehicle);
                }
            }

            return vehicles;
        }
    }
}
