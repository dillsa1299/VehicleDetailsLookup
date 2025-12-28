namespace VehicleDetailsLookup.Models.AiData
{
    internal sealed class MotPriceEstimateDataModel
    {
        public int YearOfManufacture { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string Mileage { get; set; } = string.Empty;
        public IEnumerable<string> Defects { get; set; } = [];
    }
}
