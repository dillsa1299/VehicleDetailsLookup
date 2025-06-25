using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Mappers.DatabaseFrontend
{
    /// <summary>
    /// Service for mapping database models to frontend models for vehicle details, MOT tests, images, AI data, and lookups.
    /// </summary>
    public interface IDatabaseFrontendMapperService
    {
        /// <summary>
        /// Maps a database vehicle details model to a frontend details model.
        /// </summary>
        /// <param name="details">The database details model.</param>
        /// <returns>The mapped frontend details model.</returns>
        DetailsModel MapDetails(IDetailsDbModel details);

        /// <summary>
        /// Maps a collection of database MOT test models to frontend MOT test models.
        /// </summary>
        /// <param name="motTests">The collection of database MOT test models.</param>
        /// <returns>The mapped collection of frontend MOT test models.</returns>
        IEnumerable<IMotTestModel> MapMotTests(IEnumerable<IMotTestDbModel> motTests);

        /// <summary>
        /// Maps a collection of database image models to frontend image models.
        /// </summary>
        /// <param name="images">The collection of database image models.</param>
        /// <returns>The mapped collection of frontend image models.</returns>
        IEnumerable<IImageModel> MapImages(IEnumerable<IImageDbModel> images);

        /// <summary>
        /// Maps a database AI data model to a frontend AI data model.
        /// </summary>
        /// <param name="aiData">The database AI data model.</param>
        /// <returns>The mapped frontend AI data model.</returns>
        IAiDataModel MapAiData(IAiDataDbModel aiData);

        /// <summary>
        /// Maps a database lookup model to a frontend lookup model.
        /// </summary>
        /// <param name="lookup">The database lookup model.</param>
        /// <returns>The mapped frontend lookup model.</returns>
        ILookupModel MapLookup(ILookupDbModel lookup);
    }
}
