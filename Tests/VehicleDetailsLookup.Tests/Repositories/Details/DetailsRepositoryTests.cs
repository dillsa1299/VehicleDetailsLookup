using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Repositories.Details;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Tests.Repositories.Details
{
    public class DetailsRepositoryTests : IDisposable
    {
        private readonly VehicleDbContext _dbContext;
        private readonly DetailsRepository _repository;

        public DetailsRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new VehicleDbContext(options);
            _repository = new DetailsRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task UpdateDetailsAsync_AddsNewRecord_WhenNotExists()
        {
            // Arrange
            var details = new DetailsDbModel
            {
                RegistrationNumber = "ABC123",
                DateOfLastV5CIssued = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
                MotExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)),
                MotStatus = MotStatus.Valid,
                TaxDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                TaxStatus = TaxStatus.Taxed,
                Updated = DateTime.UtcNow
            };

            // Act
            await _repository.UpdateDetailsAsync(details);

            // Assert
            var result = await _dbContext.Details.FindAsync("ABC123");

            Assert.NotNull(result);
            Assert.Equal("ABC123", result!.RegistrationNumber);
        }

        [Fact]
        public async Task UpdateDetailsAsync_UpdatesExistingRecord_WhenExists()
        {
            // Arrange
            var existing = new DetailsDbModel
            {
                RegistrationNumber = "DEF456",
                DateOfLastV5CIssued = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2)),
                MotExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                MotStatus = MotStatus.Invalid,
                TaxDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                TaxStatus = TaxStatus.Untaxed,
                Updated = DateTime.UtcNow.AddDays(-1)
            };

            _dbContext.Details.Add(existing);
            await _dbContext.SaveChangesAsync();

            var updated = new DetailsDbModel
            {
                RegistrationNumber = "DEF456",
                DateOfLastV5CIssued = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
                MotExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(12)),
                MotStatus = MotStatus.Valid,
                TaxDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)),
                TaxStatus = TaxStatus.Taxed,
                Updated = DateTime.UtcNow
            };

            // Act
            await _repository.UpdateDetailsAsync(updated);

            // Assert
            var result = await _dbContext.Details.FindAsync("DEF456");

            Assert.NotNull(result);
            Assert.Equal(MotStatus.Valid, result!.MotStatus);
            Assert.Equal(TaxStatus.Taxed, result.TaxStatus);
        }

        [Fact]
        public async Task GetDetailsAsync_ReturnsCorrectRecord_WhenExists()
        {
            // Arrange
            var details = new DetailsDbModel
            {
                RegistrationNumber = "GHI789",
                DateOfLastV5CIssued = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
                MotExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6)),
                MotStatus = MotStatus.Valid,
                TaxDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                TaxStatus = TaxStatus.Taxed,
                Updated = DateTime.UtcNow
            };

            _dbContext.Details.Add(details);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetDetailsAsync("GHI789");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GHI789", result!.RegistrationNumber);
        }

        [Fact]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetDetailsAsync("ZZZ999");

            // Assert
            Assert.Null(result);
        }
    }
}
