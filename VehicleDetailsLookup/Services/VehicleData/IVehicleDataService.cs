﻿using VehicleDetailsLookup.Shared.Models;

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

        /// <summary>
        /// Logs a vehicle lookup by registration number, recording the current UTC time.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle being looked up.</param>
        void LogLookup(string registrationNumber);

        /// <summary>
        /// Gets the number of times a vehicle has been looked up by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The number of lookups for the specified registration number.</returns>
        int GetVehicleLookupCount(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of recent vehicle lookups.
        /// </summary>
        /// <param name="count">The maximum number of recent lookups to retrieve.</param>
        /// <returns>
        /// An <see cref="IEnumerable{VehicleModel}"/> containing the most recent vehicles
        /// that have been looked up, ordered by lookup date descending.
        /// </returns>
        IEnumerable<VehicleModel> GetRecentLookups(int count);
    }
}
