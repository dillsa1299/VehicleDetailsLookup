using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Services.VehicleData
{
    public class VehicleDataService : IVehicleDataService
    {
        private static readonly Dictionary<string, VehicleModel> _vehicleCache = [];
        private static readonly Dictionary<DateTime, LookupModel> _vehicleLookupCache = [];

        public VehicleModel GetVehicle(string registration)
        {
            _vehicleCache.TryGetValue(registration, out var vehicle);

            return vehicle ?? new VehicleModel();
        }

        public void UpdateVehicle(VehicleModel vehicle)
        {               
            _vehicleCache[vehicle.RegistrationNumber] = vehicle;
        }

        public void LogLookup(VehicleModel vehicle)
        {
            var vehicleLookupModel = new LookupModel
            {
                DateTime = DateTime.UtcNow,
                RegistrationNumber = vehicle.RegistrationNumber,
                Details = $"{vehicle.YearOfManufacture} {vehicle.Make} {vehicle.Model}"
            };

            // Update the last lookup time for the vehicle
            _vehicleLookupCache[vehicleLookupModel.DateTime] = vehicleLookupModel;
        }

        public int GetVehicleLookupCount(string registrationNumber)
        {
            return _vehicleLookupCache.Count(lookup => lookup.Value.RegistrationNumber == registrationNumber);
        }

        public IEnumerable<LookupModel> GetRecentLookups(int count)
        {
            // Get lookups
            var lookups = _vehicleLookupCache.Values
                .OrderByDescending(lookup => lookup.DateTime)
                .Take(count);

            return lookups;
        }
    }
}
