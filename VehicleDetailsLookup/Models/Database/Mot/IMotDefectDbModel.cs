using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database.Mot
{
    /// <summary>
    /// Defines the structure for storing MOT defect information in the database.
    /// </summary>
    public interface IMotDefectDbModel
    {
        /// <summary>
        /// Indicates if the defect is classified as dangerous.
        /// </summary>
        bool Dangerous { get; set; }
        /// <summary>
        /// A description of the defect identified during the MOT test.
        /// </summary>
        string? Description { get; set; }
        /// <summary>
        /// The unique identifier for the MOT test associated with this defect.
        /// </summary>
        string? TestNumber { get; set; }
        /// <summary>
        /// The type of defect (e.g., advisory, major, dangerous).
        /// </summary>
        MotDefectType Type { get; set; }
    }
}