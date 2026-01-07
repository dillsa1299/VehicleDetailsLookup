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
        public VehicleModel Vehicle { get; set; } = default!;

        private readonly RegistrationInputModel _registrationInput = new();
        private bool _showError;

        private void OnLookupClear() => _registrationInput.Input = string.Empty;
        private void HideError() => _showError = false;

        private async Task LookupRegistration()
        {
            // Check if the vehicle is already loaded
            if (_registrationInput.Input.Replace(" ", "").Equals(Vehicle?.Details?.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
                return;

            await VehicleLookupEventsService.NotifyStartVehicleLookup(_registrationInput.Input, VehicleLookupType.Details);
        }

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData)
        {
            // Replace input - Allows input to match registrations passed via URL
            _registrationInput.Input = registrationNumber;

            // Only clear error after successful lookup
            if (!lookupStarted)
                _showError = !lookupStarted && Vehicle?.Details == null;

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
            VehicleLookupEventsService.OnLookupClear += OnLookupClear;
            base.OnInitialized();
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
