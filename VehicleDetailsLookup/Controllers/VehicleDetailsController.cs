using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.VehicleData;
using VehicleDetailsLookup.Services.VehicleDetails;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDetailsController(IVehicleDetailsService vehicleDetailsService, IVehicleDataService vehicleDataService) : ControllerBase
    {
        private readonly IVehicleDetailsService _vehicleDetailsService = vehicleDetailsService
            ?? throw new ArgumentNullException(nameof(vehicleDetailsService));

        private readonly IVehicleDataService _vehicleDataService = vehicleDataService
            ?? throw new ArgumentNullException(nameof(vehicleDataService));

        /// <summary>
        /// Retrieves detailed information about a vehicle using its registration number.
        /// </summary>
        /// <param name="registrationNumber">The unique registration number of the vehicle to look up.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the vehicle details if found;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid;
        /// or <see cref="NotFoundObjectResult"/> if the vehicle is not found.
        /// </returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleDetails(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(registrationNumber) || !RegexHelper.RegistrationNumber.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicle = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (vehicle == null || string.IsNullOrEmpty(vehicle.RegistrationNumber))
                return NotFound("Vehicle not found.");

            _vehicleDataService.LogLookup(registrationNumber);

            return Ok(vehicle);
        }
    }
}
