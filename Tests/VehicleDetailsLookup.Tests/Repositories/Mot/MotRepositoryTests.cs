using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Repositories.Mot;

namespace VehicleDetailsLookup.Tests.Repositories.Mot
{
    public class MotRepositoryTests : IDisposable
    {
        private readonly VehicleDbContext _dbContext;
        private readonly MotRepository _repository;

        public MotRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new VehicleDbContext(options);
            _repository = new MotRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task UpdateMotTestsAsync_AddsNewMotTest()
        {
            // Arrange
            var motTest = new MotTestDbModel
            {
                RegistrationNumber = "ABC123",
                TestNumber = "TST001",
                CompletedDate = DateTime.UtcNow,
                Passed = true,
                ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                OdometerValue = 12345,
                OdometerUnit = "mi",
                Updated = DateTime.UtcNow
            };

            // Act
            await _repository.UpdateMotTestsAsync([motTest]);

            // Assert
            var result = await _dbContext.MotTests.FirstOrDefaultAsync(m => m.TestNumber == "TST001");
            Assert.NotNull(result);
            Assert.Equal("ABC123", result!.RegistrationNumber);
        }

        [Fact]
        public async Task UpdateMotTestsAsync_UpdatesExistingMotTest()
        {
            // Arrange
            var motTest = new MotTestDbModel
            {
                RegistrationNumber = "DEF456",
                TestNumber = "TST002",
                CompletedDate = DateTime.UtcNow,
                Passed = false,
                ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                OdometerValue = 54321,
                OdometerUnit = "km",
                Updated = DateTime.UtcNow.AddMinutes(-10)
            };
            _dbContext.MotTests.Add(motTest);
            await _dbContext.SaveChangesAsync();

            var updatedTime = DateTime.UtcNow;
            motTest.Updated = updatedTime;

            // Act
            await _repository.UpdateMotTestsAsync([motTest]);

            // Assert
            var result = await _dbContext.MotTests.FirstOrDefaultAsync(m => m.TestNumber == "TST002");
            Assert.NotNull(result);
            Assert.Equal(updatedTime, result!.Updated);
        }

        [Fact]
        public async Task GetMotTestsAsync_ReturnsMotTestsByRegistrationNumber()
        {
            // Arrange
            var motTests = new List<MotTestDbModel>
            {
                new() { RegistrationNumber = "XYZ999", TestNumber = "TST003", CompletedDate = DateTime.UtcNow.AddDays(-2), Passed = true, ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)), OdometerValue = 10000, OdometerUnit = "mi", Updated = DateTime.UtcNow },
                new() { RegistrationNumber = "XYZ999", TestNumber = "TST004", CompletedDate = DateTime.UtcNow.AddDays(-1), Passed = false, ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)), OdometerValue = 11000, OdometerUnit = "mi", Updated = DateTime.UtcNow },
                new() { RegistrationNumber = "OTHER1", TestNumber = "TST005", CompletedDate = DateTime.UtcNow, Passed = true, ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)), OdometerValue = 12000, OdometerUnit = "mi", Updated = DateTime.UtcNow }
            };
            _dbContext.MotTests.AddRange(motTests);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetMotTestsAsync("XYZ999");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count());
            Assert.All(result, m => Assert.Equal("XYZ999", m.RegistrationNumber));
        }

        [Fact]
        public async Task UpdateMotTestsAsync_DoesNothing_WhenInputIsNullOrEmpty()
        {
            // Act
            await _repository.UpdateMotTestsAsync(null!);
            await _repository.UpdateMotTestsAsync([]);

            // Assert
            Assert.Empty(_dbContext.MotTests);
        }
    }
}
