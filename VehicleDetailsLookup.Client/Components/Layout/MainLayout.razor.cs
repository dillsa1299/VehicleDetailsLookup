using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VehicleDetailsLookup.Client.State;
using VehicleDetailsLookup.Client.Styling;

namespace VehicleDetailsLookup.Client.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        private VehicleLookupState LookupState { get; set; } = default!;

        [Inject]
        private IJSRuntime JsRuntime { get; set; } = default!;

        private bool _isDarkMode;
        private bool _themePreferenceLoaded;

        public string DarkLightModeButtonIcon =>
            _isDarkMode ? Icons.Material.Rounded.LightMode : Icons.Material.Outlined.DarkMode;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _themePreferenceLoaded)
            {
                return;
            }

            _isDarkMode = await TryReadInitialDarkModeAsync();
            _themePreferenceLoaded = true;
            StateHasChanged();
        }

        private async Task<bool> TryReadInitialDarkModeAsync()
        {
            try
            {
                return await JsRuntime.InvokeAsync<bool>("eval", "window.__vdlookupThemeIsDark === true");
            }
            catch (JSException)
            {
                return false;
            }
        }

        private async Task DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;

            try
            {
                await JsRuntime.InvokeVoidAsync("vdlookupApplyTheme", _isDarkMode);
                await JsRuntime.InvokeVoidAsync(
                    "localStorage.setItem",
                    ThemePreferenceStorage.LocalStorageKey,
                    ThemePreferenceStorage.ToStoredValue(_isDarkMode));
            }
            catch (JSException)
            {
                // Theme still toggles for this session even if persistence fails.
            }
        }

        private async Task OnCarIconClick() =>
            await LookupState.ClearAsync();
    }
}
