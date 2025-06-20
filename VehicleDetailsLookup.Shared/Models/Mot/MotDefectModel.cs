using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Mot
{
    public class MotDefectModel : IMotDefectModel
    {
        public string Description { get; set; } = string.Empty;
        public MotDefectType Type { get; set; }
        public bool Dangerous { get; set; }
    }
}
