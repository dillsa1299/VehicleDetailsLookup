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

        public string DarkLightModeButtonIcon =>
            _isDarkMode ? Icons.Material.Rounded.LightMode : Icons.Material.Outlined.DarkMode;

        private void DarkModeToggle() =>
            _isDarkMode = !_isDarkMode;

        private void OnCarIconClick() =>
            VehicleLookupEventsService.NotifyLookupClear();
    }
}
