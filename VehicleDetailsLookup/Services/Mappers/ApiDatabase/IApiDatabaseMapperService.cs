using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;
using VehicleDetailsLookup.Models.ApiResponses.Mot;
using VehicleDetailsLookup.Models.ApiResponses.Ves;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Services.Mappers.ApiDatabase
{
    public interface IApiDatabaseMapperService
    {
        DetailsDbModel MapDetails(VesResponseModel vesResponse, MotResponseModel motResponse);
        IEnumerable<MotTestDbModel> MapMotTests(string registrationNumber, IEnumerable<MotResponseTestModel> motTests);
        IEnumerable<ImageDbModel> MapImages(string registrationNumber, GoogleImageResponseModel googleImageResponse);
        AiDataDbModel MapAiData(string registrationNumber, AiType type, string aiResponse, string dataHash);
    }
}
