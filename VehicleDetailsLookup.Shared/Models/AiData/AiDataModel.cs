using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.AiData
{
    public class AiDataModel : IAiDataModel
    {
        public VehicleAiType Type { get; set; }
        public string? Content { get; set; } = string.Empty;
    }
}
