using Microsoft.AspNetCore.Components;
using System;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Components.UI.RegistrationInput;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Client.Components.Pages
{
    public partial class Main : IDisposable
    {
        [Parameter]
        public string? RegistrationNumber { get; set; }

        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        private VehicleModel _vehicle = new();

        private async Task StartLookup(string registrationNumber, VehicleLookupType lookupType)
        {
            switch (lookupType)
            {
                case VehicleLookupType.Details:
                    // Set the vehicle registration number immediately to provide instant feedback
                    _vehicle = new VehicleModel { RegistrationNumber = registrationNumber };
                    StateHasChanged();

                    _vehicle = await VehicleLookupService.GetVehicleDetailsAsync(registrationNumber);

                    // Update URL
                    var url = string.IsNullOrWhiteSpace(_vehicle.RegistrationNumber) ? "/" : $"/{_vehicle.RegistrationNumber}";
                    NavigationManager.NavigateTo(url, forceLoad: false);

                    // Dont waste further API calls on failed lookup
                    if (string.IsNullOrEmpty(_vehicle.RegistrationNumber)) return;

                    // Perform parallel lookups for images and AI overview, but do not await them
                    _ = VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.Images);
                    _ = VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.AiOverview);
                    break;

                case VehicleLookupType.Images:
                    _vehicle = await VehicleLookupService.GetVehicleImagesAsync(_vehicle.RegistrationNumber);
                    break;
                case VehicleLookupType.AiOverview:
                    _vehicle = await VehicleLookupService.GetVehicleAIAsync(_vehicle.RegistrationNumber, VehicleAiType.Overview);
                    break;
                case VehicleLookupType.AiCommonIssues:
                    _vehicle = await VehicleLookupService.GetVehicleAIAsync(_vehicle.RegistrationNumber, VehicleAiType.CommonIssues);
                    break;
                case VehicleLookupType.AiMotHistorySummary:
                    _vehicle = await VehicleLookupService.GetVehicleAIAsync(_vehicle.RegistrationNumber, VehicleAiType.MotHistorySummary);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lookupType), lookupType, null);
            }

            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!String.IsNullOrEmpty(RegistrationNumber)
                && OperatingSystem.IsBrowser()
                && !RegistrationNumber.Replace(" ", "").Equals(_vehicle.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
                await VehicleLookupEventsService.NotifyStartVehicleLookup(RegistrationNumber, VehicleLookupType.Details);

            await base.OnParametersSetAsync();
        }

        protected override void OnInitialized()
        {
            VehicleLookupEventsService.OnStartVehicleLookup += StartLookup;
            base.OnInitialized();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnStartVehicleLookup -= StartLookup;
            GC.SuppressFinalize(this);
        }
    }
}
