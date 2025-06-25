namespace VehicleDetailsLookup.Shared.Models.Mot
{
    public class MotTestModel : IMotTestModel
    {
        public string TestNumber { get; set; } = string.Empty;
        public DateOnly CompletedDate { get; set; }
        public bool Passed { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public long OdometerValue { get; set; }
        public string OdometerUnit { get; set; } = string.Empty;
        public IEnumerable<MotDefectModel> Defects { get; set; } = [];
    }
}
