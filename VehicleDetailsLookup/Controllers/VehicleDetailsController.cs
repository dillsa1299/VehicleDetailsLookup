using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for retrieving vehicle details and recording lookup history.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDetailsController(IVehicleDetailsService vehicleDetailsService) : ControllerBase
    {
        private readonly IVehicleDetailsService _vehicleDetailsService = vehicleDetailsService
            ?? throw new ArgumentNullException(nameof(vehicleDetailsService));

        /// <summary>
        /// Retrieves detailed information for a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the vehicle details if found;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid;
        /// <see cref="NotFoundObjectResult"/> if the vehicle details cannot be retrieved.
        /// </returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleDetails(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicleDetails = await _vehicleDetailsService.GetVehicleDetailsAsync(registrationNumber);

            if (vehicleDetails == null)
                return NotFound("Unable to retrieve vehicle details.");

            return Ok(vehicleDetails);
        }
    }
}
