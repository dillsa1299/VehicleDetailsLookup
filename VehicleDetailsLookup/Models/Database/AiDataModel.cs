using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Models.Database
{
    public class AiDataModel
    {
        public string? RegistrationNumber { get; set; }
        public VehicleAiType Type { get; set; }
        public string? GeneratedText { get; set; }
        public DateTime Updated { get; set; }
    }
}
