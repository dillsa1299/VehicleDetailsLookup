using VehicleDetailsLookup.Models.Database.Mot;

namespace VehicleDetailsLookup.Repositories.Mot
{
    /// <summary>
    /// Defines a contract for managing and retrieving MOT (Ministry of Transport) test records for vehicles from the database.
    /// </summary>
    public interface IMotRepository
    {
        /// <summary>
        /// Adds new MOT test records or updates existing ones for a vehicle in the database.
        /// </summary>
        /// <param name="motTests">A collection of MOT test records to be added or updated in the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateMotTestsAsync(IEnumerable<MotTestDbModel> motTests);

        /// <summary>
        /// Retrieves all MOT test records associated with a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// A value task that, when completed, returns a collection of <see cref="IMotTestDbModel"/> instances representing the vehicle's MOT test records,
        /// or <c>null</c> if no records exist for the specified registration number.
        /// </returns>
        ValueTask<IEnumerable<MotTestDbModel>?> GetMotTestsAsync(string registrationNumber);
    }
}
