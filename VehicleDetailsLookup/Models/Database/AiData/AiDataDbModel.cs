using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database.AiData
{
    public class AiDataDbModel : IAiDataDbModel
    {
        public string? RegistrationNumber { get; set; }
        public AiType Type { get; set; }
        public string? GeneratedText { get; set; }
        public DateTime Updated { get; set; }
    }
}
