namespace VehicleDetailsLookup.Shared.Models.Details
{
    /// <summary>
    /// Details about a vehicle, including registration, manufacturer, and status information.
    /// </summary>
    public interface IDetailsModel
    {
        /// <summary>
        /// Vehicle's registration number.
        /// </summary>
        string RegistrationNumber { get; set; }
        /// <summary>
        /// Year the vehicle was manufactured.
        /// </summary>
        int YearOfManufacture { get; set; }
        /// <summary>
        /// Manufacturer of the vehicle.
        /// </summary>
        string Make { get; set; }
        /// <summary>
        /// Model designation of the vehicle.
        /// </summary>
        string Model { get; set; }
        /// <summary>
        /// Exterior color of the vehicle.
        /// </summary>
        string Colour { get; set; }
        /// <summary>
        /// Engine capacity, typically in cubic centimeters (cc).
        /// </summary>
        string EngineCapacity { get; set; }
        /// <summary>
        /// Type of fuel the vehicle uses (e.g., petrol, diesel, electric).
        /// </summary>
        string FuelType { get; set; }
        /// <summary>
        /// Current tax status (e.g., taxed, untaxed).
        /// </summary>
        string TaxStatus { get; set; }
        /// <summary>
        /// Due date for the vehicle's tax, if available.
        /// </summary>
        DateOnly? TaxDueDate { get; set; }
        /// <summary>
        /// Current MOT status (e.g., valid, expired).
        /// </summary>
        string MotStatus { get; set; }
        /// <summary>
        /// Expiry date of the vehicle's MOT, if available.
        /// </summary>
        DateOnly? MotExpiryDate { get; set; }
        /// <summary>
        /// Date the latest V5C document was issued for the vehicle.
        /// </summary>
        DateOnly DateOfLastV5CIssued { get; set; }
        /// <summary>
        /// Month and year the vehicle was first registered.
        /// </summary>
        DateOnly MonthOfFirstRegistration { get; set; }
    }
}
