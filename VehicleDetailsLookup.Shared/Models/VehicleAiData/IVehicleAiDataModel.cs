using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.VehicleAiData
{
    /// <summary>
    /// Defines the structure for AI-generated vehicle data.
    /// </summary>
    public interface IVehicleAiDataModel
    {
        /// <summary>
        /// The date and time when the AI data was last updated.
        /// </summary>
        DateTime LastUpdated { get; set; }
        /// <summary>
        /// The type of AI-generated vehicle information.
        /// </summary>
        VehicleAiType Type { get; set; }
        /// <summary>
        /// The AI-generated content or summary.
        /// </summary>
        string? Content { get; set; }
    }
}
