namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Results and details of a vehicle's MOT test, including outcome, odometer, and defects.
    /// </summary>
    public interface IMotTestModel
    {
        /// <summary>
        /// Date the MOT test was completed.
        /// </summary>
        DateTime CompletedDate { get; set; }
        /// <summary>
        /// Indicates whether the vehicle passed the MOT test.
        /// </summary>
        bool Passed { get; set; }
        /// <summary>
        /// Expiry date of the MOT certificate.
        /// </summary>
        DateOnly ExpiryDate { get; set; }
        /// <summary>
        /// Recorded odometer value at the time of the MOT test.
        /// </summary>
        long OdometerValue { get; set; }
        /// <summary>
        /// Unit of measurement for the odometer (e.g., miles, kilometers).
        /// </summary>
        string OdometerUnit { get; set; }
        /// <summary>
        /// Collection of defects found during the MOT test.
        /// </summary>
        IEnumerable<MotDefectModel> Defects { get; set; }
    }
}
