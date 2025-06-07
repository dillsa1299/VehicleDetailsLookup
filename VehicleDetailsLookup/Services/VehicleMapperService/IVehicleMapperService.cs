using VehicleDetailsLookup.Models.Enums;
using VehicleDetailsLookup.Models.SearchResponses;
using VehicleDetailsLookup.Models.SearchResponses.ImageSearch;
using VehicleDetailsLookup.Models.SearchResponses.MotSearch;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleMapper
{
    /// <summary>
    /// Defines mapping operations for converting various vehicle search responses into a <see cref="VehicleModel"/>.
    /// </summary>
    public interface IVehicleMapperService
    {
        /// <summary>
        /// Maps VES and MOT search responses to a <see cref="VehicleModel"/> containing comprehensive vehicle details.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to populate with details.</param>
        /// <param name="vesSearchResponse">The <see cref="VesSearchResponse"/> containing vehicle and tax details.</param>
        /// <param name="motSearchResponse">The <see cref="MotSearchResponse"/> containing MOT history and details.</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> populated with details from the provided VES and MOT responses.
        /// </returns>
        VehicleModel MapDetails(VehicleModel vehicle, VesSearchResponse vesSearchResponse, MotSearchResponse motSearchResponse);

        /// <summary>
        /// Maps an image search response to a <see cref="VehicleModel"/> containing image information.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to populate with image data.</param>
        /// <param name="imageSearchResponse">The <see cref="ImageSearchResponse"/> containing image items.</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> with image data populated from the image search response.
        /// </returns>
        VehicleModel MapImages(VehicleModel vehicle, ImageSearchResponse imageSearchResponse);

        /// <summary>
        /// Maps an AI-generated response to a <see cref="VehicleModel"/> property based on the specified <see cref="AiSearchType"/>.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to populate with AI-generated data.</param>
        /// <param name="aiResponse">The AI-generated response as a string, typically containing summary, issues, or MOT history information.</param>
        /// <param name="searchType">The <see cref="AiSearchType"/> indicating which property of <see cref="VehicleModel"/> to populate (e.g., overview, common issues, MOT summary).</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> with the relevant AI-generated information set according to the specified <paramref name="searchType"/>.
        /// </returns>
        VehicleModel MapAI(VehicleModel vehicle, string aiResponse, AiSearchType searchType);
    }
}
