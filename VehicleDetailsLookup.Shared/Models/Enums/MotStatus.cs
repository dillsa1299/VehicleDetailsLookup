namespace VehicleDetailsLookup.Shared.Models.Enums
{
    /// <summary>
    /// Represents the MOT (Ministry of Transport) status of a vehicle.
    /// </summary>
    public enum MotStatus
    {
        /// <summary>
        /// The vehicle has a valid MOT.
        /// </summary>
        Valid,

        /// <summary>
        /// The vehicle has an invalid MOT.
        /// </summary>
        Invalid,

        /// <summary>
        /// No MOT details are available for the vehicle.
        /// </summary>
        NoDetails,

        /// <summary>
        /// No MOT results were found for the vehicle.
        /// </summary>
        NoResults
    }
}
