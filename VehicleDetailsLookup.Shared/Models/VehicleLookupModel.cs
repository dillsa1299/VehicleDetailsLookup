namespace VehicleDetailsLookup.Shared.Models
{
    public class VehicleLookupModel
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string RegistrationNumber { get; set; } = string.Empty;
    }
}
