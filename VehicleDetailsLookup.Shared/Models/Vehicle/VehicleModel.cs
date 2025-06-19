using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.VehicleAiData;

namespace VehicleDetailsLookup.Shared.Models.Vehicle
{
    /// <summary>
    /// Represents vehicle data, including details, MOT tests, images, and AI-generated information.
    /// </summary>
    public class VehicleModel : IVehicleModel
    {
        public DateTime DetailsLastUpdated { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public int YearOfManufacture { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string TaxStatus { get; set; } = string.Empty;
        public DateOnly? TaxDueDate { get; set; }
        public string MotStatus { get; set; } = string.Empty;
        public DateOnly? MotExpiryDate { get; set; }
        public IEnumerable<MotModel> MotTests { get; set; } = [];
        public DateOnly DateOfLastV5CIssued { get; set; }
        public DateOnly MonthOfFirstRegistration { get; set; }
        public DateTime ImagesLastUpdated { get; set; }
        public IEnumerable<ImageModel> Images { get; set; } = [];
        public IEnumerable<VehicleAiDataModel> AiData { get; set; } = [];
    }
}
