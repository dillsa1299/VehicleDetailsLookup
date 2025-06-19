using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.VehicleAiData
{
    /// <summary>
    /// Represents AI-generated data for a vehicle.
    /// </summary>
    public class VehicleAiDataModel : IVehicleAiDataModel
    {
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public VehicleAiType Type { get; set; }
        public string? Content { get; set; } = string.Empty;
    }
}
