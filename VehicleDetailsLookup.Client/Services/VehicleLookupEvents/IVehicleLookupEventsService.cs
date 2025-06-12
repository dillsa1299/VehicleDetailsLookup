using VehicleDetailsLookup.Client.Components.Enums;

namespace VehicleDetailsLookup.Client.Services.VehicleLookupEvents
{
    public interface IVehicleLookupEventsService: IDisposable
    {
        public delegate Task VehicleLookupStartEvent(string registrationNumber, VehicleLookupType lookupType);
        public delegate void VehicleLookupEvent(VehicleLookupType lookupType, bool lookupStarted);

        event VehicleLookupStartEvent OnStartVehicleLookup;
        event VehicleLookupEvent OnLookupStatusChanged;

        Task NotifyStartVehicleLookup(string registrationNumber, VehicleLookupType lookupType);
        void NotifyLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted);
        
    }
}
