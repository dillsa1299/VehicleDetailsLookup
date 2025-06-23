using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Repositories.Lookup;
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Services.Vehicle.LookupHistory;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for managing and retrieving vehicle lookup history.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLookupHistoryController(IVehicleLookupHistoryService vehicleLookupHistoryService) : ControllerBase
    {
        private readonly IVehicleLookupHistoryService _vehicleLookupHistoryService = vehicleLookupHistoryService
            ?? throw new ArgumentNullException(nameof(vehicleLookupHistoryService));

        /// <summary>
        /// Gets the total number of lookup records for a specific vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the count of lookup records if successful;
        /// <see cref="BadRequestObjectResult"/> if the registration number is invalid.
        /// </returns>
        [HttpGet("count/{registrationNumber}")]
        public async Task<IActionResult> GetVehicleLookupCountAsync(string registrationNumber)
        {
            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            int count = await vehicleLookupHistoryService.GetVehicleLookupCountAsync(registrationNumber);
            return Ok(count);
        }

        /// <summary>
        /// Retrieves a collection of the most recent vehicle lookup records.
        /// </summary>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing a collection of the most recent lookup records.
        /// </returns>
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentVehiclesAsync()
        {
            var recentLookups = await _vehicleLookupHistoryService.GetRecentLookupsAsync(10);
            return Ok(recentLookups);
        }
    }
}
