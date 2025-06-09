using VehicleDetailsLookup.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models
{
    public class VehicleAiDataModel
    {
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public VehicleAiType Type { get; set; }
        public string? Content { get; set; } = String.Empty;
    }
}
