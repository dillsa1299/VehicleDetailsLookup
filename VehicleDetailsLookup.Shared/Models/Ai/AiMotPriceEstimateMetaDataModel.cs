namespace VehicleDetailsLookup.Shared.Models.Ai
{
    public sealed class AiMotPriceEstimateMetaDataModel
    {
        public string TestNumber { get; set; } = string.Empty;
        public IEnumerable<Guid> DefectIds { get; set; } = [];
    }
}
