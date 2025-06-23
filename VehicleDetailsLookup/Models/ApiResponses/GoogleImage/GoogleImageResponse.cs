namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageResponse : IGoogleImageResponse
    {
        public IEnumerable<IGoogleImageResponseItem>? Items { get; set; }
    }
}
