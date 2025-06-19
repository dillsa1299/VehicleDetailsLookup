namespace VehicleDetailsLookup.Shared.Models.VehicleLookup
{
    /// <summary>
    /// Defines the structure for a vehicle lookup record.
    /// </summary>
    public interface IVehicleLookupModel
    {
        /// <summary>
        /// The date and time when the vehicle lookup occurred.
        /// </summary>
        DateTime DateTime { get; set; }
        /// <summary>
        /// The registration number of the vehicle that was looked up.
        /// </summary>
        string RegistrationNumber { get; set; }
        /// <summary>
        /// Additional details about the lookup.
        /// </summary>
        string Details { get; set; }
    }
}
