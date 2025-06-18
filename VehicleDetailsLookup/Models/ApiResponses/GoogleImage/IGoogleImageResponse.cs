
namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public interface IGoogleImageResponse
    {
        IEnumerable<GoogleImageResponseItem>? Items { get; set; }
    }
}