using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories.Image
{
    public class ImageRepository(VehicleDbContext dbContext) : IImageRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public async Task UpdateImagesAsync(IEnumerable<IImageDbModel> images)
        {
            var registrationNumbers = images.Select(i => i.RegistrationNumber).ToList();
            var existingImages = await _dbContext.Images
                .Where(img => registrationNumbers.Contains(img.RegistrationNumber))
                .ToListAsync();

            // Remove all existing images for the registration numbers in the provided images
            // Prevents duplicates and ensures only the latest images are stored
            _dbContext.Images.RemoveRange(existingImages);

            foreach (var image in images)
            {
                await _dbContext.Images.AddAsync((ImageDbModel)image);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<IImageDbModel>?> GetImagesAsync(string registrationNumber)
        {
            var images = await _dbContext.Images
                .Where(img => img.RegistrationNumber == registrationNumber)
                .ToListAsync();

            return images.Count == 0 ? null : images;
        }
    }
}
