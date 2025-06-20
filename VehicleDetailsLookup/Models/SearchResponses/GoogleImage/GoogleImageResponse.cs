namespace VehicleDetailsLookup.Models.SearchResponses.ImageSearch
{
    public class GoogleImageResponse : IGoogleImageResponse
    {
        public IEnumerable<IGoogleImageResponseItem>? Items { get; set; }
    }
}
