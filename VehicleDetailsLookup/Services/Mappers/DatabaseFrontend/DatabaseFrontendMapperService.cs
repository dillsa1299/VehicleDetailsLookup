using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Mappers.DatabaseFrontend
{
    public class DatabaseFrontendMapperService : IDatabaseFrontendMapperService
    {
        public DetailsModel MapDetails(DetailsDbModel details)
        {
            return new DetailsModel
            {
                RegistrationNumber = details.RegistrationNumber ?? string.Empty,
                YearOfManufacture = details.YearOfManufacture,
                Make = details.Make ?? string.Empty,
                Model = details.Model ?? string.Empty,
                Colour = details.Colour ?? string.Empty,
                EngineCapacity = details.EngineCapacity ?? string.Empty,
                FuelType = details.FuelType ?? string.Empty,
                TaxStatus = details.TaxStatus,
                TaxDueDate = details.TaxDueDate,
                MotStatus = details.MotStatus,
                MotExpiryDate = details.MotExpiryDate,
                DateOfLastV5CIssued = details.DateOfLastV5CIssued,
                MonthOfFirstRegistration = details.MonthOfFirstRegistration
            };
        }

        public IEnumerable<MotTestModel> MapMotTests(IEnumerable<MotTestDbModel> motTests)
        {
            if (motTests == null)
                return [];

            return motTests.Select(test => new MotTestModel
            {
                TestNumber = test.TestNumber ?? string.Empty,
                CompletedDate = test.CompletedDate,
                Passed = test.Passed,
                ExpiryDate = test.ExpiryDate,
                OdometerValue = test.OdometerValue,
                OdometerUnit = test.OdometerUnit ?? string.Empty,
                Defects = MapMotDefects(test.MotDefects)
            });
        }

        private static IEnumerable<MotDefectModel> MapMotDefects(IEnumerable<MotDefectDbModel> motDefects)
        {
            if (motDefects == null)
                return [];

            return motDefects.Select(defect => new MotDefectModel
            {
                Id = defect.Id,
                Description = defect.Description ?? string.Empty,
                Type = defect.Type,
                Dangerous = defect.Dangerous
            });
        }

        public IEnumerable<ImageModel> MapImages(IEnumerable<ImageDbModel> images)
        {
            if (images == null)
                return [];

            int i = 1;
            return images.Select(img => new ImageModel
            {
                Index = i++,
                Title = img.Title,
                Url = img.Url
            });
        }

        public AiDataModel MapAiData(AiDataDbModel aiData)
        {
            return new AiDataModel
            {
                Type = aiData.Type,
                Content = aiData.GeneratedText ?? string.Empty,
                MetaData = aiData.MetaData
            };
        }

        public LookupModel MapLookup(LookupDbModel lookup)
        {
            return new LookupModel
            {
                DateTime = lookup.DateTime,
                RegistrationNumber = lookup.RegistrationNumber ?? string.Empty,
                VehicleDetails = lookup.Details == null ? new DetailsModel(): MapDetails(lookup.Details)
            };
        }
    }
}
