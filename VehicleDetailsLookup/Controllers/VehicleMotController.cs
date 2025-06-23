using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.Vehicle.Mot;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for retrieving MOT test details for vehicles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMotController(IVehicleMotService vehicleMotService) : ControllerBase
    {
        private readonly IVehicleMotService _vehicleMotService = vehicleMotService
            ?? throw new ArgumentNullException(nameof(vehicleMotService));

        /// <summary>
        /// Retrieves a collection of MOT test details for a vehicle by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the MOT test details if found;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid;
        /// <see cref="NotFoundObjectResult"/> if the MOT test details cannot be retrieved.
        /// </returns>
        [HttpGet("{registrationNumber}")]
        public async Task<IActionResult> GetVehicleMotTests(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var motTests = await _vehicleMotService.GetVehicleMotTestsAsync(registrationNumber);

            if (motTests == null)
                return NotFound("Unable to retrieve MOT tests.");

            return Ok(motTests);
        }
    }
}
