using VehicleDetailsLookup.Models.Database.Details;

namespace VehicleDetailsLookup.Repositories.Details
{
    /// <summary>
    /// Provides methods for managing and retrieving vehicle details records from the database.
    /// </summary>
    public interface IDetailsRepository
    {
        /// <summary>
        /// Adds a new vehicle details record or updates an existing one in the database.
        /// </summary>
        /// <param name="details">The details model containing information about the vehicle to add or update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task UpdateDetailsAsync(IDetailsDbModel details);

        /// <summary>
        /// Retrieves the details for a specific vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve details for.</param>
        /// <returns>
        /// A value task that resolves to the details model for the specified vehicle, implementing <see cref="IDetailsDbModel"/>,
        /// or <c>null</c> if no record is found.
        /// </returns>
        public ValueTask<IDetailsDbModel?> GetDetailsAsync(string registrationNumber);
    }
}
