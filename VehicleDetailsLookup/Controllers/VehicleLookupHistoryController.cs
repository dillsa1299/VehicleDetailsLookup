using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for managing and retrieving vehicle lookup history.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLookupHistoryController(ILookupRepository lookupRepository) : ControllerBase
    {
        private readonly ILookupRepository _lookupRepository = lookupRepository
            ?? throw new ArgumentNullException(nameof(lookupRepository));

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

            return Ok(await _lookupRepository.GetVehicleLookupCountAsync(registrationNumber));
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
            return Ok(await _lookupRepository.GetRecentLookupsAsync(10));
        }
    }
}
