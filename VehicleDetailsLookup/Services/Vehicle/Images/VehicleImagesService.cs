using VehicleDetailsLookup.Services.Api.GoogleImage;
using VehicleDetailsLookup.Services.Vehicle.VehicleDetails;
using VehicleDetailsLookup.Services.VehicleData;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Services.Vehicle.VehicleImages
{
    /// <summary>
    /// Provides functionality to retrieve and cache vehicle images using vehicle details and an image search service.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VehicleImagesService"/> class.
    /// </remarks>
    /// <param name="imageSearch">The image search service used to find vehicle images.</param>
    /// <param name="vehicleDetails">The service used to retrieve vehicle details.</param>
    /// <param name="mapper">The service used to map image search results to the vehicle model.</param>
    /// <param name="data">The service used to retrieve and update vehicle data.</param>
    /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
    public class VehicleImagesService(
        IGoogleImageService imageSearch,
        IVehicleDetailsService vehicleDetails,
        IVehicleMapperService mapper,
        IVehicleDataService data) : IVehicleImagesService
    {
        private readonly IGoogleImageService _imageSearch = imageSearch ?? throw new ArgumentNullException(nameof(imageSearch));
        private readonly IVehicleDetailsService _vehicleDetails = vehicleDetails ?? throw new ArgumentNullException(nameof(vehicleDetails));
        private readonly IVehicleMapperService _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IVehicleDataService _data = data ?? throw new ArgumentNullException(nameof(data));

        /// <summary>
        /// Asynchronously retrieves vehicle images and details for the specified registration number.
        /// If images are cached and recent, returns the cached data; otherwise, fetches new images.
        /// </summary>
        /// <param name="registrationNumber">The vehicle's registration number.</param>
        /// <returns>
        /// A <see cref="VehicleModel"/> containing vehicle details and associated images.
        /// </returns>
        public async Task<VehicleModel> GetVehicleImagesAsync(string registrationNumber)
        {
            // Check if the vehicle is already cached
            var vehicle = _data.GetVehicle(registrationNumber);

            if (string.IsNullOrEmpty(vehicle.RegistrationNumber))
            {
                // No vehicle found, first try to get details
                vehicle = await _vehicleDetails.GetVehicleDetailsAsync(registrationNumber);

                // Unable to find vehicle details
                if (string.IsNullOrEmpty(vehicle.RegistrationNumber))
                    return new VehicleModel();
            }

            if (vehicle.Images.Any() && vehicle.ImagesLastUpdated > DateTime.UtcNow.AddDays(-1))
            {
                // Return cached vehicle images if they are recent enough
                return vehicle;
            }

            // Fetch images from the image search service
            var query = $"{vehicle.Colour} {vehicle.YearOfManufacture} {vehicle.Make} {vehicle.Model}";
            var imageSearchResponse = await _imageSearch.SearchImagesAsync(query);

            // If no images found, return the vehicle without updating
            if (imageSearchResponse.Items == null || !imageSearchResponse.Items.Any())
                return vehicle;

            vehicle = _mapper.MapImages(vehicle, imageSearchResponse);

            _data.UpdateVehicle(vehicle);

            return vehicle;
        }
    }
}
