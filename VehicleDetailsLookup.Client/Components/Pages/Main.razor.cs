using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Client.Components.Pages
{
    public partial class Main : IDisposable
    {
        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

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

        protected override Task OnInitializedAsync()
        {
            VehicleLookupEventsService.OnStartVehicleLookup += StartLookup;
            return base.OnInitializedAsync();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnStartVehicleLookup -= StartLookup;
            GC.SuppressFinalize(this);
        }
    }
}
