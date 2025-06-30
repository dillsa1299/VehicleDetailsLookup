using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.Pages
{
    public partial class Main : IDisposable
    {
        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public string? RegistrationNumberUrlInput { get; set; }

        private string PageTitle => string.IsNullOrEmpty(_vehicle?.Details?.RegistrationNumber) ? "Vehicle Details Lookup"
            : $"{_vehicle?.Details?.YearOfManufacture} {_vehicle?.Details?.Make} {_vehicle?.Details?.Model} | VDL";

        private bool IsRecentLookupsHidden
            => _lookupInProgress || !string.IsNullOrEmpty(_vehicle.Details?.RegistrationNumber);

        private VehicleModel _vehicle = new();
        private bool _lookupInProgress;

        private async Task StartLookup(string registrationNumber, VehicleLookupType lookupType)
        {
            switch (lookupType)
            {
                case VehicleLookupType.Details:
                    _vehicle.Details = await VehicleLookupService.GetVehicleDetailsAsync(registrationNumber);

                    // Update URL
                    var url = _vehicle.Details == null
                        ? "/"
                        : $"/{_vehicle.Details.RegistrationNumber}";
                    NavigationManager.NavigateTo(url, forceLoad: false);

                    // Dont waste further API calls on failed lookup
                    if (_vehicle.Details == null) return;

                    _vehicle.MotTests = await VehicleLookupService.GetMotTestsAsync(registrationNumber) ?? [];

                    // Perform parallel lookups for images and AI overview, but do not await them
                    _ = VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.Images);
                    _ = VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.AiOverview);
                    break;
                case VehicleLookupType.MotHistory:
                    _vehicle.MotTests = await VehicleLookupService.GetMotTestsAsync(registrationNumber) ?? [];
                    break;
                case VehicleLookupType.Images:
                    _vehicle.Images = await VehicleLookupService.GetVehicleImagesAsync(registrationNumber) ?? [];
                    break;
                case VehicleLookupType.AiOverview:
                case VehicleLookupType.AiCommonIssues:
                case VehicleLookupType.AiMotHistorySummary:
                case VehicleLookupType.AiClarksonOverview:
                case VehicleLookupType.AiClarksonCommonIssues:
                case VehicleLookupType.AiClarksonMotHistorySummary:
                    await GetAiData(registrationNumber, lookupType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lookupType), lookupType, null);
            }

            StateHasChanged();
        }

        private async Task GetAiData(string registrationNumber, VehicleLookupType lookupType)
        {
            var aiType = lookupType switch
            {
                VehicleLookupType.AiOverview => AiType.Overview,
                VehicleLookupType.AiCommonIssues => AiType.CommonIssues,
                VehicleLookupType.AiMotHistorySummary => AiType.MotHistorySummary,
                VehicleLookupType.AiClarksonOverview => AiType.ClarksonOverview,
                VehicleLookupType.AiClarksonCommonIssues => AiType.ClarksonCommonIssues,
                VehicleLookupType.AiClarksonMotHistorySummary => AiType.ClarksonMotHistorySummary,
                _ => throw new ArgumentOutOfRangeException(nameof(lookupType), lookupType, null)
            };

            var aiData = await VehicleLookupService.GetVehicleAiDataAsync(registrationNumber, aiType);

            if (aiData == null)
            {
                // If AI data is not found, clear any existing data for this type
                _vehicle.AiData.Remove(aiType);
                return;
            }

            // If AI data is found, add or update it in the vehicle model
            if (!_vehicle.AiData.TryAdd(aiType, aiData))
            {
                // Update the existing entry
                _vehicle.AiData[aiType] = aiData;
            }
        }

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
        {
            _lookupInProgress = lookupStarted;
            StateHasChanged();
        }

        private void OnLookupClear()
        {
            // Clear lookup
            _vehicle = new VehicleModel();

            // Reset URL
            NavigationManager.NavigateTo("/", forceLoad: false);

            StateHasChanged();
        }

        private void OnEasterEggActivated(bool activated)
        {
            if (activated && !string.IsNullOrWhiteSpace(_vehicle?.Details?.RegistrationNumber))
            {
                // Start AI lookups for Jeremy Clarkson impersonations
                _ = VehicleLookupEventsService.NotifyStartVehicleLookup(_vehicle.Details.RegistrationNumber, VehicleLookupType.AiClarksonOverview);
                _ = VehicleLookupEventsService.NotifyStartVehicleLookup(_vehicle.Details.RegistrationNumber, VehicleLookupType.AiClarksonCommonIssues);
                _ = VehicleLookupEventsService.NotifyStartVehicleLookup(_vehicle.Details.RegistrationNumber, VehicleLookupType.AiClarksonMotHistorySummary);
            }
            
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(RegistrationNumberUrlInput)
                && OperatingSystem.IsBrowser()
                && !RegistrationNumberUrlInput.Replace(" ", "").Equals(_vehicle?.Details?.RegistrationNumber, StringComparison.InvariantCultureIgnoreCase))
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
            VehicleLookupEventsService.OnLookupClear += OnLookupClear;
            VehicleLookupEventsService.OnEasterEggActivated += OnEasterEggActivated;
            base.OnInitialized();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnStartVehicleLookup -= StartLookup;
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
            VehicleLookupEventsService.OnLookupClear -= OnLookupClear;
            VehicleLookupEventsService.OnEasterEggActivated -= OnEasterEggActivated;
            GC.SuppressFinalize(this);
        }
    }
}
