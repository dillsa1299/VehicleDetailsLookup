using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace VehicleDetailsLookup.Client.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

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
            NavigationManager.NavigateTo("/", forceLoad: true);
        }

        public string DarkLightModeButtonIcon => _isDarkMode switch
        {
            true => Icons.Material.Rounded.LightMode,
            false => Icons.Material.Outlined.DarkMode,
        };
    }
}
