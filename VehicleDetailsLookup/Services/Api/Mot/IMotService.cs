using VehicleDetailsLookup.Models.ApiResponses.Mot;

namespace VehicleDetailsLookup.Services.Api.Mot
{
    /// <summary>
    /// Defines a contract for a service that retrieves MOT (Ministry of Transport) data for vehicles.
    /// </summary>
    public interface IMotService
    {
        /// <summary>
        /// Asynchronously retrieves the MOT response data for a specified vehicle registration number.
        /// </summary>
        /// <param name="registrationNumber">The vehicle registration number to look up MOT data for.</param>
        /// <returns>
        /// A <see cref="ValueTask{IMotResponse}"/> representing the asynchronous operation, 
        /// containing the MOT response data for the specified vehicle, or <c>null</c> if not found.
        /// </returns>
        ValueTask<IMotResponse?> GetMotResponseAsync(string registrationNumber);
    }
}
