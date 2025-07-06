using System.Collections.Generic;

namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    public class CandidateModel
    {
        public ContentModel? Content { get; set; }
        public string? FinishReason { get; set; }
        public double AvgLogprobs { get; set; }
    }
}
