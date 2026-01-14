using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Repositories.Lookup;

namespace VehicleDetailsLookup.Tests.Repositories.Lookup
{
    public class LookupRepositoryTests : IDisposable
    {
        private readonly VehicleDbContext _dbContext;
        private readonly LookupRepository _repository;

        public LookupRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new VehicleDbContext(options);
            _repository = new LookupRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task AddLookupAsync_AddsNewLookup()
        {
            // Act
            await _repository.AddLookupAsync("ABC123");

            // Assert
            var result = await _dbContext.Lookups.FirstOrDefaultAsync(l => l.RegistrationNumber == "ABC123");
            Assert.NotNull(result);
            Assert.Equal("ABC123", result!.RegistrationNumber);
        }

        [Fact(Skip = "Unable to test on in-memory DB due to executing SQL")]
        public async Task GetRecentLookupsAsync_ReturnsRecentLookups()
        {
            // Arrange
            var lookups = new List<LookupDbModel>
            {
                new() { RegistrationNumber = "AAA111", DateTime = DateTime.UtcNow.AddMinutes(-3) },
                new() { RegistrationNumber = "BBB222", DateTime = DateTime.UtcNow.AddMinutes(-2) },
                new() { RegistrationNumber = "CCC333", DateTime = DateTime.UtcNow.AddMinutes(-1) }
            };

            _dbContext.Lookups.AddRange(lookups);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetRecentLookupsAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count());
        }

        [Fact]
        public async Task GetRecentLookupsAsync_ByRegistrationNumber_ReturnsCorrectLookups()
        {
            // Arrange
            var lookups = new List<LookupDbModel>
            {
                new() { RegistrationNumber = "DEF456", DateTime = DateTime.UtcNow.AddMinutes(-2) },
                new() { RegistrationNumber = "DEF456", DateTime = DateTime.UtcNow.AddMinutes(-1) },
                new() { RegistrationNumber = "XYZ999", DateTime = DateTime.UtcNow }
            };

            _dbContext.Lookups.AddRange(lookups);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetRecentLookupsAsync("DEF456", 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count());
            Assert.All(result, l => Assert.Equal("DEF456", l.RegistrationNumber));
        }

        [Fact(Skip = "Unable to test on in-memory DB due to executing SQL")]
        public async Task GetRecentLookupsAsync_ReturnsNull_WhenNoneExist()
        {
            // Act
            var result = await _repository.GetRecentLookupsAsync(5);

            // Assert
            Assert.Empty(result ?? []);
        }

        [Fact]
        public async Task GetVehicleLookupCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            _dbContext.Lookups.Add(new LookupDbModel { RegistrationNumber = "COUNT1", DateTime = DateTime.UtcNow });
            _dbContext.Lookups.Add(new LookupDbModel { RegistrationNumber = "COUNT1", DateTime = DateTime.UtcNow.AddMinutes(-1) });
            _dbContext.Lookups.Add(new LookupDbModel { RegistrationNumber = "COUNT2", DateTime = DateTime.UtcNow });
            await _dbContext.SaveChangesAsync();

            // Act
            var count = await _repository.GetVehicleLookupCountAsync("COUNT1");

            // Assert
            Assert.Equal(2, count);
        }
    }
}
