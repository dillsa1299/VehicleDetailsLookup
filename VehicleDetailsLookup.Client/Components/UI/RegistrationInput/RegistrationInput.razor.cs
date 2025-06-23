using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.RegistrationInput
{
    public partial class RegistrationInput
    {
        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Parameter]
        public IVehicleModel Vehicle { get; set; } = default!;

        private readonly RegistrationInputModel _registrationInput = new();
        private bool _lookupFailed;

        private async Task LookupRegistration()
        {
            if (_registrationInput.Input.Replace(" ", "").Equals(Vehicle?.Details?.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                // Vehicle already loaded
                return;
            }
            await VehicleLookupEventsService.NotifyStartVehicleLookup(_registrationInput.Input, VehicleLookupType.Details);
        }

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
        {
            // Replace input - Allows input to match registrations passed via URL
            _registrationInput.Input = registrationNumber;

            if (!lookupStarted)
            {
                _lookupFailed = Vehicle?.Details == null;
            }

            StateHasChanged();
        }

        private void OnLookupClear()
        {
            _registrationInput.Input = string.Empty;
        }

        protected override Task OnInitializedAsync()
        {
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
            VehicleLookupEventsService.OnLookupClear += OnLookupClear;

            return base.OnInitializedAsync();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
            VehicleLookupEventsService.OnLookupClear -= OnLookupClear;
        }

        private class RegistrationInputModel
        {
            public string Input { get; set; } = string.Empty;
        }
    }
}
