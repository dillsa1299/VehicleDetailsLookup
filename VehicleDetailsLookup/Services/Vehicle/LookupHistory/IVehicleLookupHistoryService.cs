using VehicleDetailsLookup.Shared.Models.Lookup;

namespace VehicleDetailsLookup.Services.Vehicle.LookupHistory
{
    /// <summary>
    /// Service interface for managing and retrieving vehicle lookup history.
    /// </summary>
    public interface IVehicleLookupHistoryService
    {
        /// <summary>
        /// Gets the total number of times a vehicle has been looked up by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The count of lookups for the specified registration number.</returns>
        ValueTask<int> GetVehicleLookupCountAsync(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of the most recent vehicle lookups.
        /// </summary>
        /// <param name="count">The maximum number of recent lookups to retrieve.</param>
        /// <returns>
        /// An enumerable of <see cref="ILookupModel"/> representing the recent lookups,
        /// or <c>null</c> if no lookups are available.
        /// </returns>
        ValueTask<IEnumerable<ILookupModel>?> GetRecentLookupsAsync(int count);

        /// <summary>
        /// Retrieves a collection of the most recent vehicle lookups for a given registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <param name="count">The maximum number of recent lookups to retrieve.</param>
        /// <returns>
        /// An enumerable of <see cref="ILookupModel"/> representing the recent lookups
        /// for the specified registration number, or <c>null</c> if no lookups are available.
        /// </returns>
        ValueTask<IEnumerable<ILookupModel>?> GetRecentLookupsAsync(string registrationNumber, int count);
    }
}
