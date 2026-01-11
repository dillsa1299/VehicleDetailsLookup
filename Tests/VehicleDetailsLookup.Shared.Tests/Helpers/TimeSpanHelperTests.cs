using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Shared.Tests.Helpers
{
    public class TimeSpanHelperTests
    {
        [Theory]
        [InlineData(0, 0, false, "Just now")]
        [InlineData(0, 0, true, "now")]
        [InlineData(0, 1, false, "1 minute ago")]
        [InlineData(0, 1, true, "1m ago")]
        [InlineData(0, 30, false, "30 minutes ago")]
        [InlineData(0, 30, true, "30m ago")]
        [InlineData(1, 0, false, "1 hour ago")]
        [InlineData(1, 0, true, "1h ago")]
        [InlineData(2, 0, false, "2 hours ago")]
        [InlineData(2, 0, true, "2h ago")]
        [InlineData(24, 0, false, "1 day ago")]
        [InlineData(24, 0, true, "1d ago")]
        [InlineData(48, 0, false, "2 days ago")]
        [InlineData(48, 0, true, "2d ago")]
        public void GetTimeSpan_ReturnsExpectedString(int hoursAgo, int minutesAgo, bool simplified, string expected)
        {
            // Arrange
            var now = DateTime.UtcNow;
            var input = now.AddHours(-hoursAgo).AddMinutes(-minutesAgo);

            // Act
            var result = TimeSpanHelper.GetTimeSpan(input, simplified);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
