using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Ai
{
    public class AiDataModel
    {
        public AiType Type { get; set; }
        public string? Content { get; set; } = string.Empty;
        public string MetaData { get; set; } = string.Empty;
    }
}
