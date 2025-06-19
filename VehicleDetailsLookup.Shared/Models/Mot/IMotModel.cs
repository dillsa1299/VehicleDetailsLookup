namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Defines the structure for MOT test data.
    /// </summary>
    public interface IMotModel
    {
        /// <summary>
        /// The date the MOT test was completed.
        /// </summary>
        DateOnly CompletedDate { get; set; }
        /// <summary>
        /// Indicates whether the vehicle passed the MOT test.
        /// </summary>
        bool Passed { get; set; }
        /// <summary>
        /// The expiry date of the MOT certificate.
        /// </summary>
        DateOnly ExpiryDate { get; set; }
        /// <summary>
        /// The recorded odometer value at the time of the MOT test.
        /// </summary>
        long OdometerValue { get; set; }
        /// <summary>
        /// The unit of measurement for the odometer (e.g., miles, kilometers).
        /// </summary>
        string OdometerUnit { get; set; }
        /// <summary>
        /// The collection of defects found during the MOT test.
        /// </summary>
        IEnumerable<MotDefectModel> Defects { get; set; }
    }
}
