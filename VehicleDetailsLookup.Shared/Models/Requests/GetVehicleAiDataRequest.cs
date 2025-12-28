using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Shared.Models.Requests
{
    public sealed class GetVehicleAiDataRequest
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public AiType SearchType { get; set; }
        public string MetaData { get; set; } = string.Empty;
    }
}
