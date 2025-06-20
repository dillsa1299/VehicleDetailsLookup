using VehicleDetailsLookup.Shared.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Shared.Models.Vehicle
{
    public class VehicleModel : IVehicleModel
    {
        public IDetailsModel? Details { get; set; }
        public IEnumerable<IMotModel> MotTests { get; set; } = [];
        public IEnumerable<IImageModel> Images { get; set; } = [];
        public IEnumerable<IAiDataModel> AiData { get; set; } = [];
    }
}
