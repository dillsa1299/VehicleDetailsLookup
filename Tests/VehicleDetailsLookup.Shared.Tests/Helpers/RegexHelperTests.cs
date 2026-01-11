using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Shared.Tests.Helpers
{
    public class RegexHelperTests
    {
        [Theory]
        [InlineData("ABC123", true)]
        [InlineData("A1B2C3", true)]
        [InlineData("1234567", true)]
        [InlineData("ABCDEFG", true)]
        [InlineData("abc1234", true)]
        [InlineData("", false)]
        [InlineData("A B C 1 2 3", false)]
        [InlineData("12345678", false)]
        [InlineData("ABCDEFGH", false)]
        [InlineData("abc12345", false)]
        [InlineData("abc-123", false)]
        [InlineData("abc_123", false)]
        [InlineData("abc@123", false)]
        [InlineData("abc.123", false)]
        [InlineData("abc 123", false)]
        [InlineData("abc123!", false)]
        public void RegistrationNumber_Regex_Matches_Expected(string input, bool expected)
        {
            // Arrange
            var regex = RegexHelper.RegistrationNumber();

            // Act
            var result = regex.IsMatch(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
