namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public class MotResponseDefect : IMotResponseDefect
    {
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Dangerous { get; set; }
    }
}