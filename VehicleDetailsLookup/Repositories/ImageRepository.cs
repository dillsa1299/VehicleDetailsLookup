using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories
{
    public class ImageRepository(VehicleDbContext dbContext) : IImageRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public void UpdateImages(IEnumerable<ImageDbModel> images)
        {
            var existingImages = _dbContext.Images
                .Where(img => images.Any(i => i.RegistrationNumber == img.RegistrationNumber))
                .ToList();

            // Remove all existing images for the registration numbers in the provided images
            // Prevents duplicates and ensures only the latest images are stored
            _dbContext.Images.RemoveRange(existingImages);

            foreach (var image in images)
            {
                _dbContext.Images.Add(image);
            }
            _dbContext.SaveChanges();
        }

        public IEnumerable<IImageDbModel>? GetImages(string registrationNumber)
        {
            return _dbContext.Images
                .Where(img => img.RegistrationNumber == registrationNumber)
                .ToList();
        }
    }
}
