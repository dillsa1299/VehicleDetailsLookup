namespace VehicleDetailsLookup.Shared.Models.VehicleLookup
{
    /// <summary>
    /// Represents a record of a vehicle lookup, including the time, registration number, and details.
    /// </summary>
    public class VehicleLookupModel : IVehicleLookupModel
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
