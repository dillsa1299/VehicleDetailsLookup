using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database.AiData
{
    public class AiDataDbModel
    {
        public string? RegistrationNumber { get; set; }
        public AiType Type { get; set; }
        public string MetaData { get; set; } = string.Empty;
        public string? GeneratedText { get; set; }
        public DateTime Updated { get; set; }
        public string? DataHash { get; set; }
    }
}
