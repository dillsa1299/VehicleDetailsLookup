using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database.Details
{
    public class DetailsDbModel
    {
        public string? RegistrationNumber { get; set; }
        public int YearOfManufacture { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Colour { get; set; }
        public string? EngineCapacity { get; set; }
        public string? FuelType { get; set; }
        public TaxStatus TaxStatus { get; set; }
        public DateOnly? TaxDueDate { get; set; }
        public MotStatus MotStatus { get; set; }
        public DateOnly? MotExpiryDate { get; set; }
        public DateOnly DateOfLastV5CIssued { get; set; }
        public DateOnly MonthOfFirstRegistration { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<LookupDbModel> Lookups { get; set; } = [];
    }
}
