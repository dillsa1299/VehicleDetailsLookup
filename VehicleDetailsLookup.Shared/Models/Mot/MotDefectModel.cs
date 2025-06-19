using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Represents a defect found during an MOT test.
    /// </summary>
    public class MotDefectModel : IMotDefectModel
    {
        public string Description { get; set; } = string.Empty;
        public MotDefectType Type { get; set; }
        public bool Dangerous { get; set; }
    }
}
