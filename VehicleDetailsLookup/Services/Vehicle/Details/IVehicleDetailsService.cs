using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Services.Vehicle.Details
{
    /// <summary>
    /// Provides an abstraction for retrieving detailed information about vehicles.
    /// </summary>
    public interface IVehicleDetailsService
    {
        /// <summary>
        /// Asynchronously retrieves detailed information for a vehicle using its registration number.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle to look up.</param>
        /// <param name="primarySearch">
        /// Indicates whether this lookup is a direct user-initiated search (true) or a secondary/background details load (false).
        /// </param>
        /// <returns>
        /// A <see cref="IDetailsModel"/> instance containing the vehicle's details if found; otherwise, <c>null</c>.
        /// </returns>
        ValueTask<IDetailsModel?> GetVehicleDetailsAsync(string registrationNumber, bool primarySearch = false);
    }
}
