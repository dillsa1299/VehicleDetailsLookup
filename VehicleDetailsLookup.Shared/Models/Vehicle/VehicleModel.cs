using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Ai;

namespace VehicleDetailsLookup.Shared.Models.Vehicle
{
    public class VehicleModel
    {
        public DetailsModel? Details { get; set; }
        public IEnumerable<MotTestModel> MotTests { get; set; } = [];
        public IEnumerable<ImageModel> Images { get; set; } = [];
        public IDictionary<AiType, AiDataModel> AiData { get; set; } = new Dictionary<AiType, AiDataModel>();
    }
}
