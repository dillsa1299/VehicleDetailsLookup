using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.UI.RegistrationInput
{
    public partial class RegistrationInput
    {
        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Parameter]
        public VehicleModel Vehicle { get; set; } = default!;

        RegistrationInputModel _registrationInput = new();
        private bool _lookupFailed;

        private async Task LookupRegistration()
        {
            if (_registrationInput.Input.Replace(" ", "").Equals(Vehicle.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                // Vehicle already loaded
                return;
            }
            await VehicleLookupEventsService.NotifyStartVehicleLookup(_registrationInput.Input, VehicleLookupType.Details);
        }

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted)
        {
            if (!lookupStarted)
            {
                // Lookup completed
                if (string.IsNullOrEmpty(Vehicle.RegistrationNumber))
                {
                    // Failed lookup - Don't replace input - allows user to correct mistakes
                    _lookupFailed = true;
                }
                else
                {
                    // Replace input - Allows input to match registrations passed via URL
                    _registrationInput.Input = Vehicle.RegistrationNumber;
                    _lookupFailed = false;
                }
            }

            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;

            return base.OnInitializedAsync();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
        }

        private class RegistrationInputModel
        {
            public string Input { get; set; } = string.Empty;
        }
    }
}
