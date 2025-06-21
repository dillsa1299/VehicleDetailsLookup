using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.AiData
{
    /// <summary>
    /// AI-generated vehicle data, such as summaries or insights, categorized by type.
    /// </summary>
    public interface IAiDataModel
    {
        /// <summary>
        /// Type of AI-generated vehicle information.
        /// </summary>
        AiType Type { get; set; }
        /// <summary>
        /// AI-generated content or summary.
        /// </summary>
        string? Content { get; set; }
    }
}
