using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.AiData
{
    public class AiDataModel : IAiDataModel
    {
        public AiType Type { get; set; }
        public string? Content { get; set; } = string.Empty;
    }
}
