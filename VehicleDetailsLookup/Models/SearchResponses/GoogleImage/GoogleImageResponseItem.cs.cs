namespace VehicleDetailsLookup.Models.SearchResponses.ImageSearch
{
    public class ImageSearchItemResponse : IGoogleImageResponseItem
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
    }
}
