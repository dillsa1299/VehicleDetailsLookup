
namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public interface IMotResponse
    {
        string FirstUsedDate { get; set; }
        string FuelType { get; set; }
        string HasOutstandingRecall { get; set; }
        string Make { get; set; }
        string ManufactureDate { get; set; }
        string Model { get; set; }
        IEnumerable<IMotResponseTest> MotTests { get; set; }
        string PrimaryColour { get; set; }
        string Registration { get; set; }
        string RegistrationDate { get; set; }
    }
}