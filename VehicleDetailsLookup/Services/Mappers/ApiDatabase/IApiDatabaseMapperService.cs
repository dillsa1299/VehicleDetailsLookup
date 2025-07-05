using VehicleDetailsLookup.Models.ApiResponses.Gemini;
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
        DetailsDbModel MapDetails(IVesResponse vesResponse, IMotResponse motResponse);
        IEnumerable<MotTestDbModel> MapMotTests(string registrationNumber, IEnumerable<IMotResponseTest> motTests);
        IEnumerable<ImageDbModel> MapImages(string registrationNumber, IEnumerable<IGoogleImageResponseItem> images);
        AiDataDbModel MapAiData(string registrationNumber, AiType type, IGeminiResponse geminiResponse, string dataHash);
    }
}
