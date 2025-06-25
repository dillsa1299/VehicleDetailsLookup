using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Shared.Models.Lookup
{
    /// <summary>
    /// Record of a vehicle lookup, including the time, registration, and details.
    /// </summary>
    public interface ILookupModel
    {
        /// <summary>
        /// Date and time when the vehicle lookup occurred.
        /// </summary>
        DateTime DateTime { get; set; }
        /// <summary>
        /// Registration number of the vehicle that was looked up.
        /// </summary>
        string RegistrationNumber { get; set; }
        /// <summary>
        /// Details of the vehicle at the time of lookup.
        /// </summary>
        DetailsModel VehicleDetails { get; set; }
    }
}
