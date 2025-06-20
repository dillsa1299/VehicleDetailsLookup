using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Defect identified during an MOT test, including its description, type, and danger status.
    /// </summary>
    public interface IMotDefectModel
    {
        /// <summary>
        /// Description of the defect.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Type of defect (e.g., advisory, major, dangerous).
        /// </summary>
        MotDefectType Type { get; set; }
        /// <summary>
        /// Indicates whether the defect is classified as dangerous.
        /// </summary>
        bool Dangerous { get; set; }
    }
}
