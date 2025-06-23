using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Services.Vehicle.AiData
{
    /// <summary>
    /// Service interface for retrieving AI-generated vehicle data, such as summaries or insights,
    /// based on vehicle registration number and the type of AI data requested.
    /// </summary>
    public interface IVehicleAiDataService
    {
        /// <summary>
        /// Asynchronously retrieves AI-generated data for a vehicle, given its registration number and the desired AI data type.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <param name="searchType">The type of AI-generated information to retrieve (e.g., overview, common issues, MOT history summary).</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> containing the <see cref="IAiDataModel"/> if found; otherwise, <c>null</c>.
        /// </returns>
        ValueTask<IAiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType searchType);
    }
}
