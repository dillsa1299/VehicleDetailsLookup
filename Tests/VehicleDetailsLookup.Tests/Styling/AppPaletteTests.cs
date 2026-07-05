// Regression tests for AppPalette constants and Theme alignment so palette drift
// between MudBlazor theme configuration and shared colour constants is caught early.

using VehicleDetailsLookup.Client.Components.Layout;
using VehicleDetailsLookup.Client.Styling;
using Xunit;

namespace VehicleDetailsLookup.Tests.Styling;

public class AppPaletteTests
{
    [Theory]
    [InlineData(nameof(AppPalette.Brand), "#1d70b8")]
    [InlineData(nameof(AppPalette.Text), "#0b0c0c")]
    [InlineData(nameof(AppPalette.Success), "#0f7a52")]
    [InlineData(nameof(AppPalette.Error), "#ca3535")]
    public void Palette_constants_match_expected_values(string propertyName, string expected)
    {
        var actual = typeof(AppPalette).GetField(propertyName)!.GetValue(null) as string;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Light_theme_primary_matches_app_palette_brand()
    {
        Assert.Equal(AppPalette.Brand, Theme.theme.PaletteLight.Primary);
    }

    [Fact]
    public void Light_theme_appbar_matches_app_palette_brand()
    {
        Assert.Equal(AppPalette.Brand, Theme.theme.PaletteLight.AppbarBackground);
    }

    [Fact]
    public void Dark_theme_primary_matches_app_palette_brand()
    {
        Assert.Equal(AppPalette.Brand, Theme.theme.PaletteDark.Primary);
    }

    [Fact]
    public void Tokens_css_brand_variable_matches_app_palette()
    {
        var tokensPath = Path.Combine(
            FindRepoRoot(),
            "VehicleDetailsLookup",
            "wwwroot",
            "css",
            "tokens.css");

        var tokensCss = File.ReadAllText(tokensPath);

        Assert.Contains($"--app-color-brand: {AppPalette.Brand}", tokensCss);
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "VehicleDetailsLookup.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root from test output path.");
    }
}
