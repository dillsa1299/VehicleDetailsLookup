using VehicleDetailsLookup.Shared.Helpers;

namespace VehicleDetailsLookup.Shared.Tests.Helpers
{
    public class DataHashHelperTests
    {
        private class TestData
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void GenerateHash_SameInput_ReturnsSameHash()
        {
            // Arrange
            var data = new TestData { Id = 1, Name = "Test" };

            // Act
            var hash1 = DataHashHelper.GenerateHash(data);
            var hash2 = DataHashHelper.GenerateHash(data);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GenerateHash_DifferentInput_ReturnsDifferentHash()
        {
            // Arrange
            var data1 = new TestData { Id = 1, Name = "Test" };
            var data2 = new TestData { Id = 2, Name = "Test2" };

            // Act
            var hash1 = DataHashHelper.GenerateHash(data1);
            var hash2 = DataHashHelper.GenerateHash(data2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GenerateHash_ReturnsNonNullOrEmpty()
        {
            // Arrange
            var data = new TestData { Id = 1, Name = "Test" };

            // Act
            var hash = DataHashHelper.GenerateHash(data);

            // Assert
            Assert.False(string.IsNullOrEmpty(hash));
        }
    }
}
