
namespace VehicleDetailsLookup.Models.SearchResponses.ImageSearch
{
    public interface IGoogleImageResponse
    {
        IEnumerable<IGoogleImageResponseItem>? Items { get; set; }
    }
}