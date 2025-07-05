using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Shared.Models.Lookup
{
    /// <summary>
    /// Represents a record of a vehicle lookup, including the time, registration number, and details.
    /// </summary>
    public class LookupModel
    {
        public DateTime DateTime { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public DetailsModel VehicleDetails { get; set; } = default!;
    }
}
