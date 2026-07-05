// Maps light/dark theme preference to localStorage values shared between
// theme-boot.js, layout, and JS interop. Key strings in theme-boot.js must stay aligned.

namespace VehicleDetailsLookup.Client.Styling;

public static class ThemePreferenceStorage
{
    public const string LocalStorageKey = "vdlookup-theme-mode";
    public const string DarkValue = "dark";
    public const string LightValue = "light";

    public static bool? ParseStoredValue(string? stored) =>
        stored switch
        {
            DarkValue => true,
            LightValue => false,
            _ => null,
        };

    public static string ToStoredValue(bool isDarkMode) =>
        isDarkMode ? DarkValue : LightValue;
}
