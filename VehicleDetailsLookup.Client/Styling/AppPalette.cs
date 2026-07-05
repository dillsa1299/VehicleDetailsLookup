// Centralises application colour constants used across the MudBlazor theme and global CSS.
// Keeps palette values in one place so Theme.cs, app.css, and tests stay aligned.

namespace VehicleDetailsLookup.Client.Styling;

public static class AppPalette
{
    public const string Brand = "#1d70b8";
    public const string BrandDarken = "#16548a";
    public const string BrandLighten = "#5694ca";

    public const string Text = "#0b0c0c";
    public const string SecondaryText = "#484949";

    public const string TemplateBackground = "#f4f8fb";
    public const string BodyBackground = "#ffffff";

    public const string Border = "#cecece";

    public const string Success = "#0f7a52";
    public const string SuccessDarken = "#0b5c3e";
    public const string SuccessLighten = "#4b9b7d";

    public const string Error = "#ca3535";
    public const string ErrorDarken = "#982828";
    public const string ErrorLighten = "#d96a6a";

    public const string Warning = "#f47738";
    public const string WarningDarken = "#b75a2a";
    public const string WarningLighten = "#f79a6f";

    public const string Teal = "#158187";
    public const string TealDarken = "#10615f";
    public const string TealLighten = "#50a1a5";

    public const string Focus = "#ffdd00";

    public const string Link = "#1a65a6";
    public const string LinkHover = "#0f385c";

    public const string GrayDefault = "#858686";
    public const string GrayLight = "#cecece";
    public const string GrayLighter = "#f3f3f3";
    public const string GrayDark = "#484949";
    public const string GrayDarker = "#0b0c0c";

    public const string DarkBackground = "#1a1d20";
    public const string DarkSurface = "#2a2d31";
    public const string DarkTextPrimary = "#f3f3f3";
    public const string DarkTextSecondary = "#cecece";

    public const string White = "#ffffff";
    public const string OverlayDark = "rgba(11, 12, 12, 0.45)";
    public const string ImageCounterOverlay = "rgba(11, 12, 12, 0.6)";
}
