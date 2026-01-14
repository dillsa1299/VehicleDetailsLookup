using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;

namespace VehicleDetailsLookup.Tests.Services.Mappers.DatabaseFrontend
{
    public class DatabaseFrontendMapperServiceTests
    {
        [Fact]
        public void MapDetails_MapsCorrectly()
        {
            // Arrange
            var detailsDb = new DetailsDbModel
            {
                RegistrationNumber = "ABC123",
                YearOfManufacture = 2020,
                Make = "Ford",
                Model = "Focus",
                Colour = "Blue",
                EngineCapacity = "1600 cc",
                FuelType = "Petrol",
                TaxStatus = TaxStatus.Taxed,
                TaxDueDate = DateOnly.Parse("2024-12-01"),
                MotStatus = MotStatus.Valid,
                MotExpiryDate = DateOnly.Parse("2024-11-01"),
                DateOfLastV5CIssued = DateOnly.Parse("2023-01-01"),
                MonthOfFirstRegistration = DateOnly.Parse("2020-01-01")
            };

            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapDetails(detailsDb);

            // Assert
            Assert.Equal("ABC123", result.RegistrationNumber);
            Assert.Equal(2020, result.YearOfManufacture);
            Assert.Equal("Ford", result.Make);
            Assert.Equal("Focus", result.Model);
            Assert.Equal("Blue", result.Colour);
            Assert.Equal("1600 cc", result.EngineCapacity);
            Assert.Equal("Petrol", result.FuelType);
            Assert.Equal(TaxStatus.Taxed, result.TaxStatus);
            Assert.Equal(MotStatus.Valid, result.MotStatus);
            Assert.Equal(DateOnly.Parse("2024-12-01"), result.TaxDueDate);
            Assert.Equal(DateOnly.Parse("2024-11-01"), result.MotExpiryDate);
            Assert.Equal(DateOnly.Parse("2023-01-01"), result.DateOfLastV5CIssued);
            Assert.Equal(DateOnly.Parse("2020-01-01"), result.MonthOfFirstRegistration);
        }

        [Fact]
        public void MapMotTests_MapsCorrectly()
        {
            // Arrange
            var motDbTests = new List<MotTestDbModel>()
            {
                new() {
                    TestNumber = "1",
                    CompletedDate = new DateTime(2023, 1, 1),
                    Passed = true,
                    ExpiryDate = DateOnly.Parse("2024-01-01"),
                    OdometerValue = 12345,
                    OdometerUnit = "Kilometers",
                    MotDefects =
                    [
                        new MotDefectDbModel { Id = Guid.NewGuid(), Description = "defect1", Type = MotDefectType.Advisory, Dangerous = false },
                        new MotDefectDbModel { Id = Guid.NewGuid(), Description = "defect2", Type = MotDefectType.Fail, Dangerous = true }
                    ]
                }
            };

            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapMotTests(motDbTests).ToList();

            // Assert
            var test = Assert.Single(result);
            Assert.Equal("1", test.TestNumber);
            Assert.Equal(new DateTime(2023, 1, 1), test.CompletedDate);
            Assert.True(test.Passed);
            Assert.Equal(DateOnly.Parse("2024-01-01"), test.ExpiryDate);
            Assert.Equal(12345, test.OdometerValue);
            Assert.Equal("Kilometers", test.OdometerUnit);
            Assert.Equal(2, test.Defects.Count());
            Assert.Contains(test.Defects, d => d.Type == MotDefectType.Advisory);
            Assert.Contains(test.Defects, d => d.Type == MotDefectType.Fail);
        }

        [Fact]
        public void MapImages_MapsCorrectly()
        {
            // Arrange
            var imagesDb = new List<ImageDbModel>
            {
                new() { Title = "Car", Url = "http://img1", RegistrationNumber = "ABC123" },
                new() { Title = "", Url = "http://img2", RegistrationNumber = "ABC123" }
            };
            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapImages(imagesDb).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Index);
            Assert.Equal(2, result[1].Index);
            Assert.Equal("Car", result[0].Title);
            Assert.Equal("http://img1", result[0].Url);
            Assert.Equal("http://img2", result[1].Url);
        }

        [Fact]
        public void MapAiData_MapsCorrectly()
        {
            // Arrange
            var aiDb = new AiDataDbModel
            {
                Type = AiType.Overview,
                GeneratedText = "response",
                MetaData = "meta"
            };

            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapAiData(aiDb);

            // Assert
            Assert.Equal(AiType.Overview, result.Type);
            Assert.Equal("response", result.Content);
            Assert.Equal("meta", result.MetaData);
        }

        [Fact]
        public void MapLookup_MapsCorrectly()
        {
            // Arrange
            var lookupDb = new LookupDbModel
            {
                DateTime = new DateTime(2024, 1, 1),
                RegistrationNumber = "ABC123",
                Details = new DetailsDbModel { RegistrationNumber = "ABC123", YearOfManufacture = 2020 }
            };

            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapLookup(lookupDb);

            // Assert
            Assert.Equal(new DateTime(2024, 1, 1), result.DateTime);
            Assert.Equal("ABC123", result.RegistrationNumber);
            Assert.NotNull(result.VehicleDetails);
            Assert.Equal("ABC123", result.VehicleDetails.RegistrationNumber);
            Assert.Equal(2020, result.VehicleDetails.YearOfManufacture);
        }

        [Fact]
        public void MapMotTests_ReturnsEmpty_OnNullInput()
        {
            // Arrange
            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapMotTests(null!);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void MapImages_ReturnsEmpty_OnNullInput()
        {
            // Arrange
            var mapper = new DatabaseFrontendMapperService();

            // Act
            var result = mapper.MapImages(null!);

            // Assert
            Assert.Empty(result);
        }
    }
}
