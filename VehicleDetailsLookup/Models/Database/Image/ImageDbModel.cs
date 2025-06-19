namespace VehicleDetailsLookup.Models.Database.Image
{
    public class ImageDbModel : IImageDbModel
    {
        public string? RegistrationNumber { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
    }
}
