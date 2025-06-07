using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models
{
    public class MotDefectModel
    {
        public string Description { get; set; } = string.Empty;
        public MotDefectType Type { get; set; }
        public bool Dangerous { get; set; }
    }
}
