using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Models.AiData
{
    internal sealed class MotTestSummaryDataModel
    {
        public int YearOfManufacture { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public MotTestModel? TestModel { get; set; }
    }
}
