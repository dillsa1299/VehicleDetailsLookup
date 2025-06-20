
namespace VehicleDetailsLookup.Models.SearchResponses.MotSearch
{
    public interface IMotAuthToken
    {
        DateTime ExpireTime { get; set; }
        string Token { get; set; }
        string Type { get; set; }
    }
}