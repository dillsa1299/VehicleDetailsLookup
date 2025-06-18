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

        private event Action? OnLookupClear;
        event Action IVehicleLookupEventsService.OnLookupClear
        {
            add => OnLookupClear += value;
            remove => OnLookupClear -= value;
        }

        public void NotifyLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
        {
            OnLookupStatusChanged?.Invoke(lookupType, lookupStarted, registrationNumber);
        }

        public async Task NotifyStartVehicleLookup(string registrationNumber, VehicleLookupType lookupType)
        {
            OnLookupStatusChanged?.Invoke(lookupType, true, registrationNumber);

            // Await lookup response before invoking the next event
            if (OnStartVehicleLookup != null)
            {
                foreach (var handler in OnStartVehicleLookup.GetInvocationList())
                {
                    await ((IVehicleLookupEventsService.VehicleLookupStartEvent)handler)(registrationNumber, lookupType);
                }
            }

            OnLookupStatusChanged?.Invoke(lookupType, false, registrationNumber);
        }

        public void NotifyLookupClear()
        {
            OnLookupClear?.Invoke();
        }

        public void Dispose()
        {
            OnLookupStatusChanged = null;
            OnStartVehicleLookup = null;
            OnLookupClear = null;
            GC.SuppressFinalize(this);
        }
    }
}
