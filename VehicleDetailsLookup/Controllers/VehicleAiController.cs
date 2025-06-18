using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.VehicleAi;
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for handling AI-powered vehicle information lookups.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleAiController(IVehicleAiService vehicleAiService) : ControllerBase
    {
        private readonly IVehicleAiService _vehicleAiService = vehicleAiService
            ?? throw new ArgumentNullException(nameof(vehicleAiService));

        /// <summary>
        /// Retrieves AI-generated vehicle information based on the specified search type and registration number.
        /// </summary>
        /// <param name="searchType">The type of AI search to perform (e.g., Overview, CommonIssues, MotHistorySummary).</param>
        /// <param name="registrationNumber">The vehicle registration number to search for.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the vehicle information if found, or an error response if not.
        /// </returns>
        [HttpGet("{searchType}/{registrationNumber}")]
        public async Task<IActionResult> GetVehicleAiAsync(string searchType, string registrationNumber)
        {
            // Validate and parse searchType
            if (!Enum.TryParse<VehicleAiType>(searchType, true, out var parsedSearchType) ||
                !Enum.IsDefined(parsedSearchType))
            {
                return BadRequest("Invalid search type.");
            }

            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicle = await _vehicleAiService.SearchAiAsync(registrationNumber, parsedSearchType);

            if (vehicle == null || string.IsNullOrEmpty(vehicle.RegistrationNumber))
                return NotFound("Vehicle not found.");

            if (vehicle.AiData == null || !vehicle.AiData.Any(ai => ai.Type == parsedSearchType))
                return NotFound("Unable to generate AI data.");

            return Ok(vehicle);
        }
    }
}
