using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Services.Api.GoogleImage;
using VehicleDetailsLookup.Services.Mappers;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Shared.Models.Image;

namespace VehicleDetailsLookup.Services.Vehicle.Images
{
    public class VehicleImageService(IGoogleImageService googleImageService, IImageRepository imageRepository, IVehicleDetailsService vehicleDetailsService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleImageService
    {
        private readonly IGoogleImageService _googleImageService = googleImageService
            ?? throw new ArgumentNullException(nameof(_googleImageService));
        private readonly IImageRepository _imageRepository = imageRepository
            ?? throw new ArgumentNullException(nameof(_imageRepository));
        private readonly IVehicleDetailsService _vehicleDetailsService = vehicleDetailsService
            ?? throw new ArgumentNullException(nameof(_vehicleDetailsService));
        private readonly IApiDatabaseMapperService _apiMapper = apiMapper
            ?? throw new ArgumentNullException(nameof(apiMapper));
        private readonly IDatabaseFrontendMapperService _databaseMapper = databaseMapper
            ?? throw new ArgumentNullException(nameof(databaseMapper));

        public async ValueTask<IEnumerable<IImageModel>?> GetVehicleImagesAsync(string registrationNumber)
        {
            // Check if the vehicle images are already stored in the database
            var dbImages = _imageRepository.GetImages(registrationNumber);

            if (dbImages != null && dbImages.All(image => image.Updated > DateTime.UtcNow.AddDays(-1)))
            {
                // Return stored images if they are recent enough
                return _databaseMapper.MapImages(dbImages);
            }

            // Vehicle details are needed to search for images
            var vehicleDetails = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (vehicleDetails == null)
                // If no vehicle data is found, return null
                return null;

            // Fetch images from the image search service
            var query = $"{vehicleDetails.Colour} {vehicleDetails.YearOfManufacture} {vehicleDetails.Make} {vehicleDetails.Model}";
            var googleImageResponse = await _googleImageService.GetGoogleImageResponseAsync(query);

            if (googleImageResponse?.Items == null || !googleImageResponse.Items.Any())
                // If no images are found, return null
                return null;

            // Map the API response to database model and update the repository
            dbImages = _apiMapper.MapImages(registrationNumber, googleImageResponse.Items);
            _imageRepository.UpdateImages(dbImages);

            return _databaseMapper.MapImages(dbImages);
        }
    }
}
