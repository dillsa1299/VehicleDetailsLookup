using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.Services.SearchRegistration
{
    public interface ISearchRegistrationService
    {
        /// <summary>
        /// Searches for vehicle details based on the provided registration number.
        /// </summary>
        /// <param name="registration"><see cref="string"/></param>
        /// <returns><see cref="VehicleModel"/></returns>
        Task<VehicleModel> SearchVehicleAsync(VehicleModel vehicle, SearchType searchType);
    }
}
