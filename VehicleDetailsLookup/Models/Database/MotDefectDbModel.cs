using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database
{
    public class MotDefectDbModel
    {
        public string? TestNumber { get; set; }
        public string? Description { get; set; }
        public MotDefectType Type { get; set; }
        public bool Dangerous { get; set; }
    }
}
