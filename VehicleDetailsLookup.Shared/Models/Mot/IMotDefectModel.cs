using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Defines the structure for a defect found during an MOT test.
    /// </summary>
    public interface IMotDefectModel
    {
        /// <summary>
        /// A description of the defect.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// The type of defect (e.g., advisory, major, dangerous).
        /// </summary>
        MotDefectType Type { get; set; }
        /// <summary>
        /// Indicates if the defect is classified as dangerous.
        /// </summary>
        bool Dangerous { get; set; }
    }
}
