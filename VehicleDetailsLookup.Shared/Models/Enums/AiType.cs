﻿namespace VehicleDetailsLookup.Shared.Models.Enums
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
        /// AI-generated impersonation of Jeremy Clarkson giving an overview of the vehicle, including general information and highlights.
        /// </summary>
        ClarksonOverview,
        /// <summary>
        /// AI-generated impersonation of Jeremy Clarkson giving a list of common issues or problems associated with the vehicle model.
        /// </summary>
        ClarksonCommonIssues,
        /// <summary>
        /// AI-generated impersonation of Jeremy Clarkson giving a summary of the vehicle's MOT history.
        /// </summary>
        ClarksonMotHistorySummary
    }
}
