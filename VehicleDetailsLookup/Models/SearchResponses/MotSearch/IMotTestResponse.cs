
namespace VehicleDetailsLookup.Models.SearchResponses.MotSearch
{
    public interface IMotTestResponse
    {
        string CompletedDate { get; set; }
        string DataSource { get; set; }
        IEnumerable<IMotDefectResponse> Defects { get; set; }
        string ExpiryDate { get; set; }
        string MotTestNumber { get; set; }
        string OdometerResultType { get; set; }
        string OdometerUnit { get; set; }
        string OdometerValue { get; set; }
        string TestResult { get; set; }
    }
}