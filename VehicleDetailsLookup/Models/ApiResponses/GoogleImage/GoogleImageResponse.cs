namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageResponse : IGoogleImageResponse
    {
        public IEnumerable<GoogleImageResponseItem>? Items { get; set; }
    }
}
