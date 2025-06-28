using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Details
{
    public class DetailsModel : IDetailsModel
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public int YearOfManufacture { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public TaxStatus TaxStatus { get; set; }
        public DateOnly? TaxDueDate { get; set; }
        public MotStatus MotStatus { get; set; }
        public DateOnly? MotExpiryDate { get; set; }
        public DateOnly DateOfLastV5CIssued { get; set; }
        public DateOnly MonthOfFirstRegistration { get; set; }
    }
}
