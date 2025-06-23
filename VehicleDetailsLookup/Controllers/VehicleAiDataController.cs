using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.Vehicle.AiData;
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Controllers
{
    /// <summary>
    /// API controller for retrieving AI-generated vehicle information such as overviews, common issues, or MOT history summaries.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleAiDataController(IVehicleAiDataService vehicleAiService) : ControllerBase
    {
        private readonly IVehicleAiDataService _vehicleAiService = vehicleAiService
            ?? throw new ArgumentNullException(nameof(vehicleAiService));

        /// <summary>
        /// Retrieves AI-generated data for a vehicle by registration number and AI data type.
        /// </summary>
        /// <param name="searchType">The type of AI-generated information to retrieve (e.g., overview, common issues, MOT history summary).</param>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <returns>
        /// An <see cref="OkObjectResult"/> containing the AI-generated data if found;
        /// <see cref="BadRequestObjectResult"/> if the search type or registration number is invalid;
        /// <see cref="NotFoundObjectResult"/> if the AI-generated data cannot be retrieved.
        /// </returns>
        [HttpGet("{searchType}/{registrationNumber}")]
        public async Task<IActionResult> GetVehicleAiDataAsync(string searchType, string registrationNumber)
        {
            // Validate and parse searchType
            if (!Enum.TryParse<AiType>(searchType, true, out var parsedSearchType) ||
                !Enum.IsDefined(parsedSearchType))
            {
                return BadRequest("Invalid search type.");
            }

            // Remove whitespace and capitalize input
            registrationNumber = registrationNumber.Replace(" ", string.Empty).ToUpperInvariant();

            var regex = RegexHelper.RegistrationNumber();
            if (string.IsNullOrWhiteSpace(registrationNumber) || !regex.IsMatch(registrationNumber))
                return BadRequest("Invalid registration number.");

            var vehicleAiData = await _vehicleAiService.GetVehicleAiDataAsync(registrationNumber, parsedSearchType);

            if (vehicleAiData == null)
                return NotFound("Unable to retrieve vehicle AI data.");

            return Ok(vehicleAiData);
        }
    }
}
