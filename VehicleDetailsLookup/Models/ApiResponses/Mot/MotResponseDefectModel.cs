﻿namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public class MotResponseDefectModel
    {
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Dangerous { get; set; }
    }
}