using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.Ai;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Repositories
{
    public class AiRepository(VehicleDbContext dbContext) : IAiRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public void UpdateAi(AiDataDbModel ai)
        {
            // Try to find existing AI data for the given registration number and type
            var existing = _dbContext.AiData
                .FirstOrDefault(x => x.RegistrationNumber == ai.RegistrationNumber && x.Type == ai.Type);

            if (existing != null)
            {
                // Update existing record
                existing.GeneratedText = ai.GeneratedText;
                existing.Updated = DateTime.UtcNow;
                _dbContext.AiData.Update(existing);
            }
            else
            {
                // Add new record
                _dbContext.AiData.Add(ai);
            }
            _dbContext.SaveChanges();
        }

        public IAiDataDbModel? GetAi(string registrationNumber, AiType type)
        {
            return _dbContext.AiData
                .FirstOrDefault(ai => ai.RegistrationNumber == registrationNumber && ai.Type == type);
        }
    }
}