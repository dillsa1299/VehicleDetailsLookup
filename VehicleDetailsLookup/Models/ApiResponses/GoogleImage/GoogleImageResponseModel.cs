namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageResponseModel
    {
        public string? Kind { get; set; }
        public GoogleImageUrlModel? Url { get; set; }
        public GoogleImageQueriesModel? Queries { get; set; }
        public GoogleImageContextModel? Context { get; set; }
        public GoogleImageSearchInformationModel? SearchInformation { get; set; }
        public IEnumerable<GoogleImageItemModel>? Items { get; set; }
    }
}
