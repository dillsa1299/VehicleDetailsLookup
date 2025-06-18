namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public interface IMotResponseDefect
    {
        bool Dangerous { get; set; }
        string Text { get; set; }
        string Type { get; set; }
    }
}