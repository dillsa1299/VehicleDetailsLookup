using VehicleDetailsLookup.Models.Database.Lookup;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VehicleDetailsLookup.Repositories.Lookup
{
    /// <summary>
    /// Defines methods for managing and retrieving vehicle lookup records from the database.
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>
        /// Adds a new lookup record for the specified vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to add a lookup for.</param>
        Task AddLookupAsync(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of the most recent vehicle lookup records.
        /// </summary>
        /// <param name="count">The number of recent lookup records to retrieve.</param>
        /// <returns>
        /// A collection of the most recent lookup records implementing <see cref="ILookupDbModel"/>,
        /// or <c>null</c> if no records exist.
        /// </returns>
        ValueTask<IEnumerable<ILookupDbModel>?> GetRecentLookupsAsync(int count);

        /// <summary>
        /// Retrieves all lookup records for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve lookups for.</param>
        /// <returns>
        /// A collection of lookup records for the specified vehicle implementing <see cref="ILookupDbModel"/>,
        /// or <c>null</c> if no records exist.
        /// </returns>
        ValueTask<IEnumerable<ILookupDbModel>?> GetVehicleLookupsAsync(string registrationNumber);

        /// <summary>
        /// Gets the total number of lookup records for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// The count of lookup records for the specified vehicle.
        /// </returns>
        ValueTask<int> GetVehicleLookupCountAsync(string registrationNumber);
    }
}
