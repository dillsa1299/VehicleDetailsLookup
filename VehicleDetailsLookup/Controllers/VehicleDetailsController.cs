using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.VehicleDetailsService;

namespace VehicleDetailsLookup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDetailsController(IVehicleDetailsService vehicleDetailsService) : ControllerBase
    {
        private readonly IVehicleDetailsService _vehicleDetailsService = vehicleDetailsService
            ?? throw new ArgumentNullException(nameof(vehicleDetailsService));

        /// <summary>
        /// Gets the vehicle details based on the provided registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>A response containing the vehicle details.</returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleDetails(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return BadRequest("Registration number cannot be null or empty.");

            var vehicle = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (vehicle == null || string.IsNullOrEmpty(vehicle.RegistrationNumber))
                return NotFound("Vehicle not found.");

            return Ok(vehicle);
        }
    }
}
