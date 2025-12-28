namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public class MotResponseDefectModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Dangerous { get; set; }
    }
}