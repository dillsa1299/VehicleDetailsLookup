using VehicleDetailsLookup.Client.Components.Enums;

namespace VehicleDetailsLookup.Client.Services.VehicleLookupEvents
{
    /// <summary>
    /// Provides events and notification methods for vehicle lookup operations, enabling components to respond to lookup lifecycle changes.
    /// </summary>
    public interface IVehicleLookupEventsService : IDisposable
    {
        /// <summary>
        /// Delegate for handling the start of a vehicle lookup operation.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle being looked up.</param>
        /// <param name="lookupType">The type of lookup being performed.</param>
        /// <param name="metaData">Optional metadata associated with the lookup.</param>
        public delegate Task VehicleLookupStartEvent(string registrationNumber, VehicleLookupType lookupType, string metaData);

        /// <summary>
        /// Delegate for handling changes in the status of a vehicle lookup operation.
        /// </summary>
        /// <param name="lookupType">The type of lookup whose status changed.</param>
        /// <param name="lookupStarted">Indicates whether the lookup has started (true) or ended (false).</param>
        /// <param name="registrationNumber">The registration number of the vehicle involved in the lookup.</param>
        /// <param name="metaData">Optional metadata associated with the lookup.</param>
        public delegate void VehicleLookupEvent(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData);

        /// <summary>
        /// Represents a method that handles the activation of an Easter egg.
        /// </summary>
        /// <param name="activated">A value indicating whether the Easter egg has been activated.  <see langword="true"/> if activated;
        /// otherwise, <see langword="false"/>.</param>
        public delegate void EasterEggActivatedEvent(bool activated);

        /// <summary>
        /// Event triggered when a vehicle lookup is initiated.
        /// </summary>
        event VehicleLookupStartEvent OnStartVehicleLookup;

        /// <summary>
        /// Event triggered when the status of a vehicle lookup changes (started or ended).
        /// </summary>
        event VehicleLookupEvent OnLookupStatusChanged;

        /// <summary>
        /// Event triggered when the vehicle lookup state is cleared.
        /// </summary>
        event Action OnLookupClear;

        /// <summary>
        /// Event triggered when an Easter Egg is activated.
        /// </summary>
        event EasterEggActivatedEvent OnEasterEggActivated;

        /// <summary>
        /// Notifies subscribers to start a vehicle lookup.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle being looked up.</param>
        /// <param name="lookupType">The type of lookup being performed.</param>
        /// <param name="metaData">Optional metadata associated with the lookup.</param>
        Task NotifyStartVehicleLookup(string registrationNumber, VehicleLookupType lookupType, string metaData = "");

        /// <summary>
        /// Notifies subscribers of a change in the status of a vehicle lookup operation.
        /// </summary>
        /// <param name="lookupType">The type of lookup whose status changed.</param>
        /// <param name="lookupStarted">Indicates whether the lookup has started (true) or ended (false).</param>
        /// <param name="registrationNumber">The registration number of the vehicle involved in the lookup.</param>
        /// <param name="metaData">Optional metadata associated with the lookup.</param>
        void NotifyLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData);

        /// <summary>
        /// Notifies subscribers to clear lookup data.
        /// </summary>
        void NotifyLookupClear();

        /// <summary>
        /// Start searches for Easter Egg content.
        /// </summary>
        /// <param name="activated">Indicates whether the Easter egg has been activated.</param>
        void NotifyEasterEggActivated(bool activated);
    }
}
