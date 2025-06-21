using VehicleDetailsLookup.Models.ApiResponses.Ves;

namespace VehicleDetailsLookup.Services.Api.Ves
{
    /// <summary>
    /// Defines a contract for a service that retrieves vehicle details from the government Vehicle Enquiry Service (VES) API.
    /// </summary>
    public interface IVesService
    {
        /// <summary>
        /// Asynchronously retrieves vehicle details for the specified registration number from the VES API.
        /// </summary>
        /// <param name="registrationNumber">The vehicle registration number to look up.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, with a result of <see cref="IVesResponse"/>
        /// containing the vehicle details if found; otherwise, <c>null</c>.
        /// </returns>
        ValueTask<IVesResponse?> GetVesResponseAsync(string registrationNumber);
    }
}
