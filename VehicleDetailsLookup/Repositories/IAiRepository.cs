using VehicleDetailsLookup.Models.Database.Ai;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving AI-related vehicle data from the database.
    /// </summary>
    public interface IAiRepository
    {
        /// <summary>
        /// Adds or updates AI data for a vehicle in the database.
        /// </summary>
        /// <param name="ai">The AI data model to add or update.</param>
        public void UpdateAi(AiDataDbModel ai);

        /// <summary>
        /// Retrieves AI data for a specific vehicle registration number and AI type.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to retrieve AI data for.</param>
        /// <param name="type">The type of AI data to retrieve.</param>
        /// <returns>The AI data model for the specified vehicle and type, or null if not found.</returns>
        public IAiDataDbModel? GetAi(string registrationNumber, AiType type);
    }
}
