using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Repositories.AiData;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Tests.Repositories.AiData
{
    public class AiDataRepositoryTests : IDisposable
    {
        private readonly VehicleDbContext _dbContext;
        private readonly AiDataRepository _repository;

        public AiDataRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new VehicleDbContext(options);
            _repository = new AiDataRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task UpsertAiDataAsync_AddsNewRecord_WhenNotExists()
        {
            // Arrange
            var aiData = new AiDataDbModel
            {
                RegistrationNumber = "ABC123",
                Type = AiType.Overview,
                MetaData = "meta1",
                GeneratedText = "text",
                DataHash = "hash",
                Updated = DateTime.UtcNow
            };

            // Act
            await _repository.UpsertAiDataAsync(aiData);

            // Assert
            var result = await _dbContext.AiData.FindAsync("ABC123", AiType.Overview, "meta1");

            Assert.NotNull(result);
            Assert.Equal("ABC123", result!.RegistrationNumber);
        }

        [Fact]
        public async Task UpsertAiDataAsync_UpdatesExistingRecord_WhenExists()
        {
            // Arrange
            var existing = new AiDataDbModel
            {
                RegistrationNumber = "DEF456",
                Type = AiType.CommonIssues,
                MetaData = "meta2",
                GeneratedText = "old",
                DataHash = "oldhash",
                Updated = DateTime.UtcNow.AddDays(-1)
            };
            _dbContext.AiData.Add(existing);
            await _dbContext.SaveChangesAsync();

            var aiData = new AiDataDbModel
            {
                RegistrationNumber = "DEF456",
                Type = AiType.CommonIssues,
                MetaData = "meta2",
                GeneratedText = "new",
                DataHash = "newhash",
                Updated = DateTime.UtcNow
            };

            // Act
            await _repository.UpsertAiDataAsync(aiData);

            // Assert
            var result = await _dbContext.AiData.FindAsync("DEF456", AiType.CommonIssues, "meta2");
            Assert.NotNull(result);
            Assert.Equal("new", result!.GeneratedText);
            Assert.Equal("newhash", result.DataHash);
        }

        [Fact]
        public async Task GetAiDataAsync_ReturnsCorrectRecord_WhenExists()
        {
            // Arrange
            var aiData = new AiDataDbModel
            {
                RegistrationNumber = "GHI789",
                Type = AiType.MotHistorySummary,
                MetaData = "meta3",
                GeneratedText = "text",
                DataHash = "hash",
                Updated = DateTime.UtcNow
            };
            _dbContext.AiData.Add(aiData);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAiDataAsync("GHI789", AiType.MotHistorySummary, "meta3");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GHI789", result!.RegistrationNumber);
        }

        [Fact]
        public async Task GetAiDataAsync_ReturnsNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetAiDataAsync("ZZZ999", AiType.Overview, "metaX");

            // Assert
            Assert.Null(result);
        }
    }
}
