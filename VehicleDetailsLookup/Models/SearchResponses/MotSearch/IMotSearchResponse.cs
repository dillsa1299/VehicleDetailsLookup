
namespace VehicleDetailsLookup.Models.SearchResponses.MotSearch
{
    public interface IMotSearchResponse
    {
        string FirstUsedDate { get; set; }
        string FuelType { get; set; }
        string HasOutstandingRecall { get; set; }
        string Make { get; set; }
        string ManufactureDate { get; set; }
        string Model { get; set; }
        IEnumerable<IMotTestResponse> MotTests { get; set; }
        string PrimaryColour { get; set; }
        string Registration { get; set; }
        string RegistrationDate { get; set; }
    }
}