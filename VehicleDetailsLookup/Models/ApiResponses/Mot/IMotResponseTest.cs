namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    /// <summary>
    /// Represents a single MOT test record for a vehicle from the government MOT API.
    /// </summary>
    public interface IMotResponseTest
    {
        /// <summary>
        /// Date the MOT test was completed.
        /// </summary>
        string CompletedDate { get; set; }

        /// <summary>
        /// Source of the MOT test data.
        /// </summary>
        string DataSource { get; set; }

        /// <summary>
        /// List of defects identified during the MOT test.
        /// </summary>
        IEnumerable<IMotResponseDefect> Defects { get; set; }

        /// <summary>
        /// Expiry date of the MOT certificate.
        /// </summary>
        string ExpiryDate { get; set; }

        /// <summary>
        /// Unique MOT test number.
        /// </summary>
        string MotTestNumber { get; set; }

        /// <summary>
        /// Type of odometer result (e.g., reading, not readable).
        /// </summary>
        string OdometerResultType { get; set; }

        /// <summary>
        /// Unit of the odometer value (e.g., miles, kilometers).
        /// </summary>
        string OdometerUnit { get; set; }

        /// <summary>
        /// Odometer reading at the time of the test.
        /// </summary>
        string OdometerValue { get; set; }

        /// <summary>
        /// Result of the MOT test (e.g., pass, fail).
        /// </summary>
        string TestResult { get; set; }
    }
}