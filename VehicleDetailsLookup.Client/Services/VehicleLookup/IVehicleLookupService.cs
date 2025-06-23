using VehicleDetailsLookup.Shared.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Client.Services.VehicleLookup
{
    /// <summary>
    /// Provides methods for retrieving vehicle-related data from the backend, including vehicle details, images, AI-generated insights, MOT history, and lookup statistics.
    /// </summary>
    public interface IVehicleLookupService
    {
        /// <summary>
        /// Retrieves detailed information about a vehicle asynchronously using its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is an <see cref="IDetailsModel"/> containing vehicle details, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IDetailsModel?> GetVehicleDetailsAsync(string registrationNumber);

        /// <summary>
        /// Retrieves the MOT test history for a vehicle asynchronously by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is a collection of <see cref="IMotTestModel"/> instances, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IEnumerable<IMotTestModel>?> GetMotTestsAsync(string registrationNumber);

        /// <summary>
        /// Retrieves all images associated with a vehicle asynchronously by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is a collection of <see cref="IImageModel"/> instances, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IEnumerable<IImageModel>?> GetVehicleImagesAsync(string registrationNumber);

        /// <summary>
        /// Retrieves AI-generated data for a vehicle asynchronously, filtered by registration number and AI data type.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <param name="type">The <see cref="AiType"/> specifying the category of AI-generated information to retrieve.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is an <see cref="IAiDataModel"/> with the requested AI data, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IAiDataModel?> GetVehicleAiDataAsync(string registrationNumber, AiType type);

        /// <summary>
        /// Retrieves the total number of times a vehicle has been looked up asynchronously, identified by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is the lookup count as an <see cref="int"/>, or <c>null</c> if not found.
        /// </returns>
        ValueTask<int?> GetVehicleLookupCountAsync(string registrationNumber);

        /// <summary>
        /// Retrieves a collection of recent vehicle lookup records asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> whose result is a collection of <see cref="ILookupModel"/> instances, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IEnumerable<ILookupModel>?> GetRecentVehicleLookupsAsync();
    }
}
