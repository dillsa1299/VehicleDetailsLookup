using VehicleDetailsLookup.Models.Enums;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleAiService
{
    /// <summary>
    /// Defines an interface for AI-powered vehicle information lookup services.
    /// </summary>
    public interface IVehicleAiService
    {
        /// <summary>
        /// Searches for AI-generated vehicle information based on the registration number and search type.
        /// </summary>
        /// <param name="registrationNumber">The vehicle registration number to search for.</param>
        /// <param name="searchType">The type of AI search to perform (e.g., Overview, CommonIssues, MotHistorySummary).</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing the requested AI-generated information for the vehicle.
        /// </returns>
        Task<VehicleModel> SearchAiAsync(string registrationNumber, VehicleAiType searchType);
    }
}
