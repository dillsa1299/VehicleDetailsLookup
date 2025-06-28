namespace VehicleDetailsLookup.Shared.Models.Enums
{
    /// <summary>
    /// Represents the tax status of a vehicle.
    /// </summary>
    public enum TaxStatus
    {
        /// <summary>
        /// The vehicle is currently taxed.
        /// </summary>
        Taxed,

        /// <summary>
        /// The vehicle is not taxed.
        /// </summary>
        Untaxed,

        /// <summary>
        /// The vehicle is registered as SORN (Statutory Off Road Notification).
        /// </summary>
        Sorn
    }
}
