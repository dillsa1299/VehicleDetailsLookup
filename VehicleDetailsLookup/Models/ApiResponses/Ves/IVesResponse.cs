namespace VehicleDetailsLookup.Models.ApiResponses.Ves
{
    public interface IVesResponse
    {
        string ArtEndDate { get; set; }
        int Co2Emissions { get; set; }
        string Colour { get; set; }
        string DateOfLastV5CIssued { get; set; }
        int EngineCapacity { get; set; }
        string EuroStatus { get; set; }
        string FuelType { get; set; }
        string Make { get; set; }
        bool MarkedForExport { get; set; }
        string MonthOfFirstRegistration { get; set; }
        string MotExpiryDate { get; set; }
        string MotStatus { get; set; }
        string RealDrivingEmissions { get; set; }
        string RegistrationNumber { get; set; }
        int RevenueWeight { get; set; }
        string TaxDueDate { get; set; }
        string TaxStatus { get; set; }
        string TypeApproval { get; set; }
        string Wheelplan { get; set; }
        int YearOfManufacture { get; set; }
    }
}