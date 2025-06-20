using VehicleDetailsLookup.Models.Database.Details;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving vehicle details records from the database.
    /// </summary>
    public interface IDetailsRepository
    {
        /// <summary>
        /// Adds or updates the details of a vehicle in the database.
        /// </summary>
        /// <param name="details">The details model containing information about the vehicle.</param>
        public void UpdateDetails(DetailsDbModel details);

        /// <summary>
        /// Retrieves the details for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve details for.</param>
        /// <returns>The details model for the specified vehicle, implementing <see cref="IDetailsDbModel"/>, or null if not found.</returns>
        public IDetailsDbModel? GetDetails(string registrationNumber);
    }
}
