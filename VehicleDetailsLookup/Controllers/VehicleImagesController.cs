using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.Vehicle.Images;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for retrieving images associated with a specific vehicle.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleImagesController(IVehicleImageService vehicleImagesService) : ControllerBase
    {
        private readonly IVehicleImageService _vehicleImagesService = vehicleImagesService
            ?? throw new ArgumentNullException(nameof(vehicleImagesService));

        /// <summary>
        /// Retrieves images for a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the vehicle images if found;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid;
        /// <see cref="NotFoundObjectResult"/> if the images cannot be retrieved.
        /// </returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleImagesAsync(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicleImages = await _vehicleImagesService.GetVehicleImagesAsync(registrationNumber);

            if (vehicleImages == null)
                return NotFound("Unable to retrieve vehicle images.");

            return Ok(vehicleImages);
        }
    }
}
