namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    /// <summary>
    /// Represents a defect found during an MOT test from the government MOT API.
    /// </summary>
    public interface IMotResponseDefect
    {
        /// <summary>
        /// Indicates if the defect is dangerous.
        /// </summary>
        bool Dangerous { get; set; }

        /// <summary>
        /// Description of the defect.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Type of defect (e.g., advisory, major, dangerous).
        /// </summary>
        string Type { get; set; }
    }
}