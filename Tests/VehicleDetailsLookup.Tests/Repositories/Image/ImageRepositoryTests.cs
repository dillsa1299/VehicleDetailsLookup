using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Repositories.Image;

namespace VehicleDetailsLookup.Tests.Repositories.Image
{
    public class ImageRepositoryTests : IDisposable
    {
        private readonly VehicleDbContext _dbContext;
        private readonly ImageRepository _repository;

        public ImageRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VehicleDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new VehicleDbContext(options);
            _repository = new ImageRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task UpdateImagesAsync_AddsNewImages_WhenNoneExist()
        {
            // Arrange
            var images = new List<ImageDbModel>
            {
                new() { RegistrationNumber = "ABC123", Title = "Front", Url = "url1", Updated = DateTime.UtcNow },
                new() { RegistrationNumber = "ABC123", Title = "Rear", Url = "url2", Updated = DateTime.UtcNow }
            };

            // Act
            await _repository.UpdateImagesAsync(images);

            // Assert
            var result = await _dbContext.Images.Where(i => i.RegistrationNumber == "ABC123").ToListAsync();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, i => i.Title == "Front");
            Assert.Contains(result, i => i.Title == "Rear");
        }

        [Fact]
        public async Task UpdateImagesAsync_ReplacesExistingImages()
        {
            // Arrange
            var initialImages = new List<ImageDbModel>
            {
                new() { RegistrationNumber = "DEF456", Title = "Old1", Url = "oldurl1", Updated = DateTime.UtcNow }
            };

            _dbContext.Images.AddRange(initialImages);
            await _dbContext.SaveChangesAsync();

            var newImages = new List<ImageDbModel>
            {
                new() { RegistrationNumber = "DEF456", Title = "New1", Url = "newurl1", Updated = DateTime.UtcNow },
                new() { RegistrationNumber = "DEF456", Title = "New2", Url = "newurl2", Updated = DateTime.UtcNow }
            };

            // Act
            await _repository.UpdateImagesAsync(newImages);

            // Assert
            var result = await _dbContext.Images.Where(i => i.RegistrationNumber == "DEF456").ToListAsync();
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, i => i.Title == "Old1");
            Assert.Contains(result, i => i.Title == "New1");
            Assert.Contains(result, i => i.Title == "New2");
        }

        [Fact]
        public async Task GetImagesAsync_ReturnsImages_WhenExist()
        {
            // Arrange
            var images = new List<ImageDbModel>
            {
                new() { RegistrationNumber = "GHI789", Title = "Front", Url = "url1", Updated = DateTime.UtcNow },
                new() { RegistrationNumber = "GHI789", Title = "Rear", Url = "url2", Updated = DateTime.UtcNow }
            };

            _dbContext.Images.AddRange(images);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetImagesAsync("GHI789");

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Contains(resultList, i => i.Title == "Front");
            Assert.Contains(resultList, i => i.Title == "Rear");
        }

        [Fact]
        public async Task GetImagesAsync_ReturnsNull_WhenNoneExist()
        {
            // Act
            var result = await _repository.GetImagesAsync("ZZZ999");

            // Assert
            Assert.Null(result);
        }
    }
}
