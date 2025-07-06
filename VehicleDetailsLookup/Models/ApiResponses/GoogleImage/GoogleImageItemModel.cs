namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageItemModel
    {
        public string? Kind { get; set; }
        public string? Title { get; set; }
        public string? HtmlTitle { get; set; }
        public string? Link { get; set; }
        public string? DisplayLink { get; set; }
        public string? Snippet { get; set; }
        public string? HtmlSnippet { get; set; }
        public string? Mime { get; set; }
        public string? FileFormat { get; set; }
        public GoogleImageItemImageModel? Image { get; set; }
    }

    public class GoogleImageItemImageModel
    {
        public string? ContextLink { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? ByteSize { get; set; }
        public string? ThumbnailLink { get; set; }
        public int? ThumbnailHeight { get; set; }
        public int? ThumbnailWidth { get; set; }
    }
}
