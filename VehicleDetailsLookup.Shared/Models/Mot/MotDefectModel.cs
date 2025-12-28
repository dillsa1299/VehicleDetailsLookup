using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Mot
{
    public class MotDefectModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public MotDefectType Type { get; set; }
        public bool Dangerous { get; set; }
    }
}
