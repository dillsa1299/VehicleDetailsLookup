using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Client.Components.Pages
{
    public partial class Main : IDisposable
    {
        [Parameter]
        public string? RegistrationNumberUrlInput { get; set; }

        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private string PageTitle => string.IsNullOrEmpty(_vehicle?.RegistrationNumber) ? "Vehicle Details Lookup"
            : $"{_vehicle?.YearOfManufacture} {_vehicle?.Make} {_vehicle?.Model} | VDL";

        private bool IsRecentLookupsHidden
            => _lookupInProgress || !string.IsNullOrEmpty(_vehicle.RegistrationNumber);

        private VehicleModel _vehicle = new();
        private bool _lookupInProgress;

        private async Task StartLookup(string registrationNumber, VehicleLookupType lookupType)
        {
            switch (lookupType)
            {
                case VehicleLookupType.Details:
                    _vehicle = await VehicleLookupService.GetVehicleDetailsAsync(registrationNumber);

                    // Update URL if vehicle found
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

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
        {
            _lookupInProgress = lookupStarted;
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!String.IsNullOrEmpty(RegistrationNumberUrlInput)
                && OperatingSystem.IsBrowser()
                && !RegistrationNumberUrlInput.Replace(" ", "").Equals(_vehicle.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
                await VehicleLookupEventsService.NotifyStartVehicleLookup(RegistrationNumberUrlInput, VehicleLookupType.Details);

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("hideLoader");
            }
        }

        protected override void OnInitialized()
        {
            VehicleLookupEventsService.OnStartVehicleLookup += StartLookup;
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
            base.OnInitialized();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnStartVehicleLookup -= StartLookup;
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
            GC.SuppressFinalize(this);
        }
    }
}
