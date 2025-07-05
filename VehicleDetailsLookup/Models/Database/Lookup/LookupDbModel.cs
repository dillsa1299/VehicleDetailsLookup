using VehicleDetailsLookup.Models.Database.Details;

namespace VehicleDetailsLookup.Models.Database.Lookup
{
    public class LookupDbModel
    {
        public DateTime DateTime { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public DetailsDbModel? Details { get; set; }
    }
}
