using VehicleDetailsLookup.Models.Database.Ai;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.AiData;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Mappers
{
    public interface IDatabaseFrontendMapperService
    {
        IDetailsModel MapDetails(IDetailsDbModel details);
        IEnumerable<IMotTestModel> MapMotTests(IEnumerable<IMotTestDbModel> motTests);
        IEnumerable<IImageModel> MapImages(IEnumerable<IImageDbModel> images);
        IAiDataModel MapAi(IAiDataDbModel aiData);
    }
}
