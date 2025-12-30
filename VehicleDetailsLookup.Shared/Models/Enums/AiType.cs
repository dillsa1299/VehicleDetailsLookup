namespace VehicleDetailsLookup.Shared.Models.Enums
{
    /// <summary>
    /// Specifies the types of AI-generated information available.
    /// </summary>
    public enum AiType
    {
        /// <summary>
        /// AI-generated overview of the vehicle, including general information and highlights.
        /// </summary>
        Overview,
        /// <summary>
        /// AI-generated list of common issues or problems associated with the vehicle model.
        /// </summary>
        CommonIssues,
        /// <summary>
        /// AI-generated summary of the vehicle's MOT history.
        /// </summary>
        MotHistorySummary,
        /// <summary>
        /// AI-generated summary of a specific MOT test.
        /// </summary>
        MotTestSummary,
        /// <summary>
        /// AI-generated price estimate for repairs based on selected MOT defects.
        /// </summary>
        MotPriceEstimate
    }
}
