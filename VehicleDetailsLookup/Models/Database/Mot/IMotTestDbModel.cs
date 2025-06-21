namespace VehicleDetailsLookup.Models.Database.Mot
{
    /// <summary>
    /// Defines the structure for storing MOT test information in the database.
    /// </summary>
    public interface IMotTestDbModel
    {
        /// <summary>
        /// The date the MOT test was completed.
        /// </summary>
        DateOnly CompletedDate { get; set; }
        /// <summary>
        /// The expiry date of the MOT certificate.
        /// </summary>
        DateOnly ExpiryDate { get; set; }
        /// <summary>
        /// The unit of measurement for the odometer (e.g., miles, kilometers).
        /// </summary>
        string? OdometerUnit { get; set; }
        /// <summary>
        /// The recorded odometer value at the time of the MOT test.
        /// </summary>
        long OdometerValue { get; set; }
        /// <summary>
        /// Indicates whether the vehicle passed the MOT test.
        /// </summary>
        bool Passed { get; set; }
        /// <summary>
        /// The vehicle's registration number.
        /// </summary>
        string? RegistrationNumber { get; set; }
        /// <summary>
        /// The unique identifier for the MOT test.
        /// </summary>
        string? TestNumber { get; set; }
        /// <summary>
        /// Navigation property for related defects identified during the MOT test.
        /// </summary>
        ICollection<MotDefectDbModel> MotDefects { get; set; }
        /// <summary>
        /// When the database record was created.
        /// </summary>
        DateTime Updated { get; set; }
    }
}