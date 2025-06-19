namespace VehicleDetailsLookup.Shared.Models.Mot
{
    /// <summary>
    /// Represents MOT test data for a vehicle.
    /// </summary>
    public class MotModel : IMotModel
    {
        public DateOnly CompletedDate { get; set; }
        public bool Passed { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public long OdometerValue { get; set; }
        public string OdometerUnit { get; set; } = string.Empty;
        public IEnumerable<MotDefectModel> Defects { get; set; } = [];
    }
}
