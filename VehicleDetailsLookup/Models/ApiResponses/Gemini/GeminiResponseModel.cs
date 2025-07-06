namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    public class GeminiResponseModel
    {
        public List<CandidateModel>? Candidates { get; set; }
        public UsageMetadataModel? UsageMetadata { get; set; }
        public string? ModelVersion { get; set; }
        public string? ResponseId { get; set; }
    }
}
