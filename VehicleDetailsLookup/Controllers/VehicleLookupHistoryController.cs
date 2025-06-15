using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.VehicleData;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for handling vehicle lookup history operations.
    /// Provides endpoints to retrieve lookup counts and recent vehicle lookups.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLookupHistoryController : ControllerBase
    {
        private readonly IVehicleDataService _vehicleDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleLookupHistoryController"/> class.
        /// </summary>
        /// <param name="vehicleDataService">The service for vehicle data operations.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleDataService"/> is null.</exception>
        public VehicleLookupHistoryController(IVehicleDataService vehicleDataService)
        {
            _vehicleDataService = vehicleDataService ?? throw new ArgumentNullException(nameof(vehicleDataService));
        }

        /// <summary>
        /// Gets the number of times a vehicle has been looked up by its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>The number of lookups for the specified registration number.</returns>
        [HttpGet("count/{registrationNumber}")]
        public async Task<IActionResult> GetVehicleLookupCountAsync(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(registrationNumber) || !RegexHelper.RegistrationNumber.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            // Temporary delay to simulate async operation when using DB
            await Task.Delay(1);

            int count = _vehicleDataService.GetVehicleLookupCount(registrationNumber);

            return Ok(count);
        }

        /// <summary>
        /// Gets a list of the most recent vehicle lookups.
        /// </summary>
        /// <param name="registrationNumber">The registration number to validate (not used for filtering).</param>
        /// <returns>A list of recent vehicle lookups.</returns>
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentVehiclesAsync(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(registrationNumber) || !RegexHelper.RegistrationNumber.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            // Temporary delay to simulate async operation when using DB
            await Task.Delay(1);

            var vehicles = _vehicleDataService.GetRecentLookups(10).ToList();

            return Ok(vehicles);
        }
    }
}
