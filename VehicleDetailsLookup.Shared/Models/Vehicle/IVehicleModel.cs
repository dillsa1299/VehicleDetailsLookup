using VehicleDetailsLookup.Shared.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Shared.Models.Vehicle
{
    /// <summary>
    /// Structure for vehicle data, including details, MOT tests, images, and AI-generated information.
    /// </summary>
    public interface IVehicleModel
    {
        /// <summary>
        /// Detailed information about the vehicle.
        /// </summary>
        IDetailsModel? Details { get; set; }

        /// <summary>
        /// Collection of MOT test data for the vehicle.
        /// </summary>
        IEnumerable<IMotTestModel> MotTests { get; set; }

        /// <summary>
        /// Collection of images associated with the vehicle.
        /// </summary>
        IEnumerable<IImageModel> Images { get; set; }

        /// <summary>
        /// Collection of AI-generated data related to the vehicle.
        /// </summary>
        IEnumerable<IAiDataModel> AiData { get; set; }
    }
}
