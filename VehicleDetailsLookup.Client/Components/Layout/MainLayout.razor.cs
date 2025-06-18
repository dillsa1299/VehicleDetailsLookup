using Microsoft.AspNetCore.Components;
using MudBlazor;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;

namespace VehicleDetailsLookup.Client.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        private bool _isDarkMode = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private void DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;
        }

        private void OnCarIconClick()
        {
            VehicleLookupEventsService.NotifyLookupClear();
        }

        public string DarkLightModeButtonIcon => _isDarkMode switch
        {
            true => Icons.Material.Rounded.LightMode,
            false => Icons.Material.Outlined.DarkMode,
        };
    }
}
