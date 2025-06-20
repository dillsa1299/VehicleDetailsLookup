using VehicleDetailsLookup.Models.Database.Lookup;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving vehicle lookup records from the database.
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>
        /// Adds a new lookup record for the specified vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to add a lookup for.</param>
        void AddLookup(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of the most recent vehicle lookup records.
        /// </summary>
        /// <param name="count">The number of recent lookup records to retrieve.</param>
        /// <returns>A collection of recent lookup records implementing <see cref="ILookupDbModel"/>, or null if none exist.</returns>
        IEnumerable<ILookupDbModel>? GetRecentLookups(int count);

        /// <summary>
        /// Retrieves all lookup records for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve lookups for.</param>
        /// <returns>A collection of lookup records for the specified vehicle implementing <see cref="ILookupDbModel"/>, or null if none exist.</returns>
        IEnumerable<ILookupDbModel>? GetVehicleLookups(string registrationNumber);

        /// <summary>
        /// Gets the total number of lookup records for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The count of lookup records for the specified vehicle.</returns>
        int GetVehicleLookupCount(string registrationNumber);
    }
}
