using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;
using VehicleDetailsLookup.Models.ApiResponses.Mot;
using VehicleDetailsLookup.Models.ApiResponses.Ves;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;

namespace VehicleDetailsLookup.Tests.Services.Mappers.ApiDatabase
{
    public class ApiDatabaseMapperServiceTests
    {
        [Fact]
        public void MapDetails_MapsCorrectly()
        {
            // Arrange
            var ves = new VesResponseModel
            {
                RegistrationNumber = "ABC123",
                YearOfManufacture = 2020,
                Make = "ford",
                Colour = "blue",
                EngineCapacity = 1600,
                FuelType = "petrol",
                TaxStatus = "TAXED",
                TaxDueDate = "2024-12-01",
                MotStatus = "VALID",
                MotExpiryDate = "2024-11-01",
                DateOfLastV5CIssued = "2023-01-01",
                MonthOfFirstRegistration = "2020-01"
            };
            var mot = new MotResponseModel
            {
                Model = "focus"
            };

            var mapper = new ApiDatabaseMapperService();

            // Act
            var result = mapper.MapDetails(ves, mot);

            // Assert
            Assert.Equal("ABC123", result.RegistrationNumber);
            Assert.Equal(2020, result.YearOfManufacture);
            Assert.Equal("Ford", result.Make); // Title case
            Assert.Equal("Focus", result.Model); // Title case
            Assert.Equal("Blue", result.Colour); // Title case
            Assert.Equal("1600 cc", result.EngineCapacity);
            Assert.Equal("Petrol", result.FuelType); // Title case
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
            var motTests = new List<MotResponseTestModel>
            {
                new() {
                    MotTestNumber = "1",
                    CompletedDate = "2023-01-01",
                    TestResult = "PASSED",
                    ExpiryDate = "2024-01-01",
                    OdometerValue = "12345",
                    OdometerUnit = "KM",
                    Defects =
                    [
                        new MotResponseDefectModel { Id = Guid.NewGuid(), Text = "defect1", Type = "ADVISORY", Dangerous = false },
                        new MotResponseDefectModel { Id = Guid.NewGuid(), Text = "defect2", Type = "FAIL", Dangerous = true }
                    ]
                }
            };

            var mapper = new ApiDatabaseMapperService();

            // Act
            var result = mapper.MapMotTests("ABC123", motTests).ToList();

            // Assert
            var test = Assert.Single(result);
            Assert.Equal("ABC123", test.RegistrationNumber);
            Assert.Equal("1", test.TestNumber);
            Assert.Equal(new DateTime(2023, 1, 1), test.CompletedDate);
            Assert.True(test.Passed);
            Assert.Equal(DateOnly.Parse("2024-01-01"), test.ExpiryDate);
            Assert.Equal(12345, test.OdometerValue);
            Assert.Equal("Kilometers", test.OdometerUnit);
            Assert.Equal(2, test.MotDefects.Count);
            Assert.Contains(test.MotDefects, d => d.Type == MotDefectType.Advisory);
            Assert.Contains(test.MotDefects, d => d.Type == MotDefectType.Fail);
        }

        [Fact]
        public void MapImages_MapsCorrectly()
        {
            // Arrange
            var googleResponse = new GoogleImageResponseModel
            {
                Items =
                [
                    new GoogleImageItemModel { Title = "Car", Link = "http://img1" },
                    new GoogleImageItemModel { Title = "", Link = "http://img2" },
                    new GoogleImageItemModel { Title = "NoLink", Link = null }
                ]
            };

            var mapper = new ApiDatabaseMapperService();

            // Act
            var result = mapper.MapImages("ABC123", googleResponse).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, img => Assert.Equal("ABC123", img.RegistrationNumber));
            Assert.Contains(result, img => img.Url == "http://img1");
            Assert.Contains(result, img => img.Url == "http://img2");
        }

        [Fact]
        public void MapAiData_MapsCorrectly()
        {
            // Arrange
            var mapper = new ApiDatabaseMapperService();

            // Act
            var result = mapper.MapAiData("ABC123", AiType.Overview, "meta", "response", "hash");

            // Assert
            Assert.Equal("ABC123", result.RegistrationNumber);
            Assert.Equal(AiType.Overview, result.Type);
            Assert.Equal("meta", result.MetaData);
            Assert.Equal("response", result.GeneratedText);
            Assert.Equal("hash", result.DataHash);
        }

        [Fact]
        public void MapMotTests_ReturnsEmpty_OnNullInput()
        {
            // Arrange
            var mapper = new ApiDatabaseMapperService();

            // Act
            var result = mapper.MapMotTests("ABC123", null!);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void MapImages_ReturnsEmpty_OnNullOrEmptyItems()
        {
            // Arrange
            var mapper = new ApiDatabaseMapperService();
            var emptyResponse = new GoogleImageResponseModel { Items = null };
            var emptyListResponse = new GoogleImageResponseModel { Items = [] };

            // Act
            var result1 = mapper.MapImages("ABC123", emptyResponse);
            var result2 = mapper.MapImages("ABC123", emptyListResponse);

            // Assert
            Assert.Empty(result1);
            Assert.Empty(result2);
        }
    }
}
