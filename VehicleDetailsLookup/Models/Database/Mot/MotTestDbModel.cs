namespace VehicleDetailsLookup.Models.Database.Mot
{
    public class MotTestDbModel
    {
        public string? RegistrationNumber { get; set; }
        public string? TestNumber { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool Passed { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public long OdometerValue { get; set; }
        public string? OdometerUnit { get; set; }
        public ICollection<MotDefectDbModel> MotDefects { get; set; } = [];
        public DateTime Updated { get; set; }
    }
}
