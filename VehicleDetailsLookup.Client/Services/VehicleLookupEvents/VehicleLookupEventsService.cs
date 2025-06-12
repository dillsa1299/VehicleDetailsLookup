using VehicleDetailsLookup.Client.Components.Enums;

namespace VehicleDetailsLookup.Client.Services.VehicleLookupEvents
{
    public class VehicleLookupEventsService : IVehicleLookupEventsService
    {
        private event IVehicleLookupEventsService.VehicleLookupEvent? OnLookupStatusChanged;
        event IVehicleLookupEventsService.VehicleLookupEvent IVehicleLookupEventsService.OnLookupStatusChanged
        {
            add => OnLookupStatusChanged += value;
            remove => OnLookupStatusChanged -= value;
        }

        private event IVehicleLookupEventsService.VehicleLookupStartEvent? OnStartVehicleLookup;
        event IVehicleLookupEventsService.VehicleLookupStartEvent IVehicleLookupEventsService.OnStartVehicleLookup
        {
            add => OnStartVehicleLookup += value;
            remove => OnStartVehicleLookup -= value;
        }

        public void NotifyLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted)
        {
            OnLookupStatusChanged?.Invoke(lookupType, lookupStarted);
        }

        public async Task NotifyStartVehicleLookup(string registrationNumber, VehicleLookupType lookupType)
        {
            OnLookupStatusChanged?.Invoke(lookupType, true);

            // Await lookup response before invoking the next event
            if (OnStartVehicleLookup != null)
            {
                foreach (var handler in OnStartVehicleLookup.GetInvocationList())
                {
                    await ((IVehicleLookupEventsService.VehicleLookupStartEvent)handler)(registrationNumber, lookupType);
                }
            }

            OnLookupStatusChanged?.Invoke(lookupType, false);
        }

        public void Dispose()
        {
            OnLookupStatusChanged = null;
            OnStartVehicleLookup = null;
            GC.SuppressFinalize(this);
        }
    }
}
