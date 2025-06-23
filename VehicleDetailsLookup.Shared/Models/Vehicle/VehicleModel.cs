using System.Collections.Generic;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Ai;

namespace VehicleDetailsLookup.Shared.Models.Vehicle
{
    public class VehicleModel : IVehicleModel
    {
        public IDetailsModel? Details { get; set; }
        public IEnumerable<IMotTestModel> MotTests { get; set; } = [];
        public IEnumerable<IImageModel> Images { get; set; } = [];
        public Dictionary<AiType, IAiDataModel> AiData { get; set; } = [];
    }
}
