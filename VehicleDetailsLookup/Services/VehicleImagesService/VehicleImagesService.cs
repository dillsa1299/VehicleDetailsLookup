using VehicleDetailsLookup.Services.ImageSearchService;
using VehicleDetailsLookup.Services.VehicleDataService;
using VehicleDetailsLookup.Services.VehicleDetailsService;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Services.VehicleImagesService
{
    public class VehicleImagesService(IImageSearchService imageSearch, IVehicleDetailsService vehicleDetails, IVehicleMapperService mapper, IVehicleDataService data) : IVehicleImagesService
    {
        private readonly IImageSearchService _imageSearch = imageSearch
            ?? throw new ArgumentNullException(nameof(imageSearch));
        private readonly IVehicleDetailsService _vehicleDetails = vehicleDetails
            ?? throw new ArgumentNullException(nameof(vehicleDetails));
        private readonly IVehicleMapperService _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IVehicleDataService _data = data
            ?? throw new ArgumentNullException(nameof(data));

        public async Task<VehicleModel> GetVehicleImagesAsync(string registrationNumber)
        {
            // Check if the vehicle is already cached
            var vehicle = _data.GetVehicle(registrationNumber);

            if (String.IsNullOrEmpty(vehicle.RegistrationNumber))
            {
                // No vehicle found, first try to get details
                vehicle = await _vehicleDetails.GetVehicleDetailsAsync(registrationNumber);

                // Unable to find vehicle details
                if (String.IsNullOrEmpty(vehicle.RegistrationNumber))
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
