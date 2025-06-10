using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.VehicleImagesService;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleImageController(IVehicleImagesService vehicleImagesService) : ControllerBase
    {
        private readonly IVehicleImagesService _vehicleImagesService = vehicleImagesService
            ?? throw new ArgumentNullException(nameof(vehicleImagesService));

        /// <summary>
        /// Gets the vehicle model with images based on the provided registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the vehicle details if found;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid;
        /// or <see cref="NotFoundObjectResult"/> if the vehicle is not found.
        /// </returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleImagesAsync(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(registrationNumber) || !RegexHelper.RegistrationNumber.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicle = await _vehicleImagesService.GetVehicleImagesAsync(registrationNumber);

            if (vehicle == null || string.IsNullOrEmpty(vehicle.RegistrationNumber))
                return NotFound("Vehicle not found.");

            if (vehicle.Images == null || !vehicle.Images.Any())
                return NotFound("Unable to source images.");

            return Ok(vehicle);
        }
    }
}
