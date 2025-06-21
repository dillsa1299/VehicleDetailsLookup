namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    public class MotAuthToken : IMotAuthToken
    {
        public string Type { get; set; } = string.Empty;
        public DateTime ExpireTime { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
