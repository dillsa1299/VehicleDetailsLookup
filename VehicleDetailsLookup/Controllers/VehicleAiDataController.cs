using Microsoft.AspNetCore.Mvc;
using VehicleDetailsLookup.Services.Vehicle.AiData;
using VehicleDetailsLookup.Shared.Models.Requests;

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
        /// Handles a POST request to retrieve AI-generated data for a vehicle based on the specified request parameters.
        /// </summary>
        /// <param name="request">The request object containing the parameters required to obtain vehicle AI data. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vehicle AI data if found; otherwise, a NotFound result if the
        /// data cannot be retrieved.</returns>
        [HttpPost]
        public async Task<IActionResult> GetVehicleAiDataAsync([FromBody] GetVehicleAiDataRequest request)
        {
            var vehicleAiData = await _vehicleAiService.GetVehicleAiDataAsync(request);

            if (vehicleAiData == null)
                return NotFound("Unable to retrieve vehicle AI data.");

            return Ok(vehicleAiData);
        }
    }
}
