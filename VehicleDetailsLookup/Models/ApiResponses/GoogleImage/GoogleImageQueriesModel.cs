namespace VehicleDetailsLookup.Models.ApiResponses.GoogleImage
{
    public class GoogleImageQueriesModel
    {
        public IEnumerable<GoogleImageQueryPageModel>? Request { get; set; }
        public IEnumerable<GoogleImageQueryPageModel>? NextPage { get; set; }
    }
}