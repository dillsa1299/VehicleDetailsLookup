namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageQueryPageModel
    {
        public string? Title { get; set; }
        public string? TotalResults { get; set; }
        public string? SearchTerms { get; set; }
        public int? Count { get; set; }
        public int? StartIndex { get; set; }
        public string? InputEncoding { get; set; }
        public string? OutputEncoding { get; set; }
        public string? Safe { get; set; }
        public string? Cx { get; set; }
        public string? SearchType { get; set; }
    }
}