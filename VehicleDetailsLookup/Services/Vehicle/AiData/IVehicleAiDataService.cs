using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Requests;

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
        /// <param name="request">The request containing the vehicle registration number and AI data type.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> containing the <see cref="IAiDataModel"/> if found; otherwise, <c>null</c>.
        /// </returns>
        ValueTask<AiDataModel?> GetVehicleAiDataAsync(GetVehicleAiDataRequest request);
    }
}
