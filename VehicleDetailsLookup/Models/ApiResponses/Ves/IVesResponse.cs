namespace VehicleDetailsLookup.Models.ApiResponses.Ves
{
    /// <summary>
    /// Response data for a vehicle's details from the government VES (Vehicle Enquiry Service) API.
    /// </summary>
    public interface IVesResponse
    {
        /// <summary>
        /// End date of the annual road tax (ART).
        /// </summary>
        string ArtEndDate { get; set; }

        /// <summary>
        /// CO2 emissions value.
        /// </summary>
        int Co2Emissions { get; set; }

        /// <summary>
        /// Vehicle colour.
        /// </summary>
        string Colour { get; set; }

        /// <summary>
        /// Date the last V5C was issued.
        /// </summary>
        string DateOfLastV5CIssued { get; set; }

        /// <summary>
        /// Engine capacity in cubic centimeters.
        /// </summary>
        int EngineCapacity { get; set; }

        /// <summary>
        /// Euro emissions status.
        /// </summary>
        string EuroStatus { get; set; }

        /// <summary>
        /// Type of fuel the vehicle uses.
        /// </summary>
        string FuelType { get; set; }

        /// <summary>
        /// Vehicle make.
        /// </summary>
        string Make { get; set; }

        /// <summary>
        /// Indicates if the vehicle is marked for export.
        /// </summary>
        bool MarkedForExport { get; set; }

        /// <summary>
        /// Month of first registration.
        /// </summary>
        string MonthOfFirstRegistration { get; set; }

        /// <summary>
        /// MOT certificate expiry date.
        /// </summary>
        string MotExpiryDate { get; set; }

        /// <summary>
        /// MOT status (e.g., valid, expired).
        /// </summary>
        string MotStatus { get; set; }

        /// <summary>
        /// Real driving emissions value.
        /// </summary>
        string RealDrivingEmissions { get; set; }

        /// <summary>
        /// Vehicle registration number.
        /// </summary>
        string RegistrationNumber { get; set; }

        /// <summary>
        /// Revenue weight of the vehicle.
        /// </summary>
        int RevenueWeight { get; set; }

        /// <summary>
        /// Date when vehicle tax is due.
        /// </summary>
        string TaxDueDate { get; set; }

        /// <summary>
        /// Tax status (e.g., taxed, untaxed).
        /// </summary>
        string TaxStatus { get; set; }

        /// <summary>
        /// Type approval code.
        /// </summary>
        string TypeApproval { get; set; }

        /// <summary>
        /// Vehicle wheelplan (e.g., 2-axle rigid body).
        /// </summary>
        string Wheelplan { get; set; }

        /// <summary>
        /// Year the vehicle was manufactured.
        /// </summary>
        int YearOfManufacture { get; set; }
    }
}