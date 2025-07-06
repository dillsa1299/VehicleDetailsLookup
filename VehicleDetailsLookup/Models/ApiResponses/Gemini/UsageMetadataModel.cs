using System.Collections.Generic;

namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    public class UsageMetadataModel
    {
        public int PromptTokenCount { get; set; }
        public int CandidatesTokenCount { get; set; }
        public int TotalTokenCount { get; set; }
        public List<TokenDetailsModel>? PromptTokensDetails { get; set; }
        public List<TokenDetailsModel>? CandidatesTokensDetails { get; set; }
    }
}
