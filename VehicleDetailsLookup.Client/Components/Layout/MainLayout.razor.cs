using Microsoft.AspNetCore.Components;
using MudBlazor;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;

namespace VehicleDetailsLookup.Client.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        private bool _isDarkMode = true;
        private int _toggleCount = 0;
        private bool _clarksonEasterEggEnabled;

        public string DarkLightModeButtonIcon =>
            _isDarkMode ? Icons.Material.Rounded.LightMode : Icons.Material.Outlined.DarkMode;

        private void DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;

            if (++_toggleCount >= 10)
            {
                _toggleCount = 0;
                _clarksonEasterEggEnabled = !_clarksonEasterEggEnabled;
                VehicleLookupEventsService.NotifyEasterEggActivated(_clarksonEasterEggEnabled);
            }
        }

        private void OnCarIconClick()
        {
            VehicleLookupEventsService.NotifyLookupClear();
        }

        private string TitleIcon()
        {
            if (_clarksonEasterEggEnabled)
                return "<image width=\"100%\" height=\"100%\" xlink:href=\"images/jeremy-clarkson.png\" />";

            return Icons.Material.Filled.DirectionsCar;
        }

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData)
        {
            _clarksonEasterEggEnabled = lookupType switch
            {
                VehicleLookupType.AiClarksonOverview or VehicleLookupType.AiClarksonCommonIssues or VehicleLookupType.AiClarksonMotHistorySummary => true,
                _ => false
            };

            if (!_clarksonEasterEggEnabled)
                // Notify that the Easter egg is deactivated
                VehicleLookupEventsService.NotifyEasterEggActivated(false);

            // Reset toggle count if a lookup happens
            _toggleCount = 0;
        }

        private void OnLookupClear()
        {
            _toggleCount = 0;
            _clarksonEasterEggEnabled = false;
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
    }
}
