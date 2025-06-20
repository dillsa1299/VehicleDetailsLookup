namespace VehicleDetailsLookup.Models.SearchResponses.MotSearch
{
    public interface IMotDefectResponse
    {
        bool Dangerous { get; set; }
        string Text { get; set; }
        string Type { get; set; }
    }
}