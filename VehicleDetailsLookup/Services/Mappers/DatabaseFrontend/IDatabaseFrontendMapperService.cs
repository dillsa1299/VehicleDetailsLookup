using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Mappers.DatabaseFrontend
{
    public interface IDatabaseFrontendMapperService
    {
        IDetailsModel MapDetails(IDetailsDbModel details);
        IEnumerable<IMotTestModel> MapMotTests(IEnumerable<IMotTestDbModel> motTests);
        IEnumerable<IImageModel> MapImages(IEnumerable<IImageDbModel> images);
        IAiDataModel MapAiData(IAiDataDbModel aiData);
    }
}
