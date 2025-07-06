using System.Collections.Generic;

namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    public class ContentModel
    {
        public List<PartModel>? Parts { get; set; }
        public string? Role { get; set; }
    }
}
