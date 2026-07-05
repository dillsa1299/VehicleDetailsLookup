using VehicleDetailsLookup.Client.Styling;
using Xunit;

namespace VehicleDetailsLookup.Tests.Styling;

public class ThemePreferenceStorageTests
{
    [Theory]
    [InlineData("dark", true)]
    [InlineData("light", false)]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    public void ParseStoredValue_maps_known_values(string? stored, bool? expected)
    {
        Assert.Equal(expected, ThemePreferenceStorage.ParseStoredValue(stored));
    }

    [Theory]
    [InlineData(true, "dark")]
    [InlineData(false, "light")]
    public void ToStoredValue_round_trips_with_parse(bool isDarkMode, string expected)
    {
        var stored = ThemePreferenceStorage.ToStoredValue(isDarkMode);

        Assert.Equal(expected, stored);
        Assert.Equal(isDarkMode, ThemePreferenceStorage.ParseStoredValue(stored));
    }
}
