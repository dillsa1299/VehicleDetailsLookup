using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database.Ai
{
    public class AiDbModel : IAiDbModel
    {
        public string? RegistrationNumber { get; set; }
        public VehicleAiType Type { get; set; }
        public string? GeneratedText { get; set; }
        public DateTime Updated { get; set; }
    }
}
