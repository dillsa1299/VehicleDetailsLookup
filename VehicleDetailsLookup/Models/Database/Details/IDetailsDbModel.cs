namespace VehicleDetailsLookup.Models.Database.Details
{
    /// <summary>
    /// Defines the structure for storing vehicle details in the database.
    /// </summary>
    public interface IDetailsDbModel
    {
        /// <summary>
        /// The exterior color of the vehicle.
        /// </summary>
        string? Colour { get; set; }
        /// <summary>
        /// The date the latest V5C document was issued for the vehicle.
        /// </summary>
        DateOnly DateOfLastV5CIssued { get; set; }
        /// <summary>
        /// The engine capacity, typically in cubic centimeters (cc).
        /// </summary>
        string? EngineCapacity { get; set; }
        /// <summary>
        /// The type of fuel the vehicle uses (e.g., petrol, diesel, electric).
        /// </summary>
        string? FuelType { get; set; }
        /// <summary>
        /// The manufacturer of the vehicle.
        /// </summary>
        string? Make { get; set; }
        /// <summary>
        /// The model designation of the vehicle.
        /// </summary>
        string? Model { get; set; }
        /// <summary>
        /// The month and year the vehicle was first registered.
        /// </summary>
        DateOnly MonthOfFirstRegistration { get; set; }
        /// <summary>
        /// The expiry date of the vehicle's MOT, if available.
        /// </summary>
        DateOnly? MotExpiryDate { get; set; }
        /// <summary>
        /// The current MOT status (e.g., valid, expired).
        /// </summary>
        string? MotStatus { get; set; }
        /// <summary>
        /// The vehicle's registration number.
        /// </summary>
        string? RegistrationNumber { get; set; }
        /// <summary>
        /// The due date for the vehicle's tax, if available.
        /// </summary>
        DateOnly? TaxDueDate { get; set; }
        /// <summary>
        /// The current tax status (e.g., taxed, untaxed).
        /// </summary>
        string? TaxStatus { get; set; }
        /// <summary>
        /// The date and time when the record was last updated.
        /// </summary>
        DateTime Updated { get; set; }
        /// <summary>
        /// The year the vehicle was manufactured.
        /// </summary>
        int YearOfManufacture { get; set; }
    }
}