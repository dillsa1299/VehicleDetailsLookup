using VehicleDetailsLookup.Models.Database.Mot;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving MOT test records for vehicles from the database.
    /// </summary>
    public interface IMotRepository
    {
        /// <summary>
        /// Adds or updates the MOT tests for a vehicle in the database using a collection of MOT test records.
        /// </summary>
        /// <param name="motTests">A collection of MOT test records to be stored or updated in the database.</param>
        void UpdateMotTests(IEnumerable<MotTestDbModel> motTests);

        /// <summary>
        /// Retrieves the MOT tests for a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>A collection of <see cref="IMotTestDbModel"/> instances representing the vehicle's MOT test records, or null if none exist.</returns>
        IEnumerable<IMotTestDbModel>? GetMotTests(string registrationNumber);
    }
}
