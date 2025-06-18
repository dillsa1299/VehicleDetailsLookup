
namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public interface IMotResponseTest
    {
        string CompletedDate { get; set; }
        string DataSource { get; set; }
        IEnumerable<IMotResponseDefect> Defects { get; set; }
        string ExpiryDate { get; set; }
        string MotTestNumber { get; set; }
        string OdometerResultType { get; set; }
        string OdometerUnit { get; set; }
        string OdometerValue { get; set; }
        string TestResult { get; set; }
    }
}