﻿namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public class MotResponseTestModel
    {
        public string CompletedDate { get; set; } = string.Empty;
        public string TestResult { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string OdometerValue { get; set; } = string.Empty;
        public string OdometerUnit { get; set; } = string.Empty;
        public string OdometerResultType { get; set; } = string.Empty;
        public string MotTestNumber { get; set; } = string.Empty;
        public string DataSource { get; set; } = string.Empty;
        public IEnumerable<MotResponseDefectModel> Defects { get; set; } = [];
    }
}
