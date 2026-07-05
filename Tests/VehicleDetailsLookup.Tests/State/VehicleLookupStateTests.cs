using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.State;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;
using Xunit;

namespace VehicleDetailsLookup.Tests.State;

public class VehicleLookupStateTests
{
    [Fact]
    public async Task IsSearching_is_true_during_lookup_and_false_after_completion()
    {
        var service = new FakeVehicleLookupService
        {
            ImagesDelay = TimeSpan.FromMilliseconds(50),
        };
        var navigation = new TestNavigationManager();
        var state = new VehicleLookupState(service, navigation);

        var lookupTask = state.StartLookupAsync("AB12CDE", VehicleLookupType.Images);

        Assert.True(state.IsSearching(VehicleLookupType.Images));
        Assert.True(state.IsLookupInProgress);

        await lookupTask;

        Assert.False(state.IsSearching(VehicleLookupType.Images));
        Assert.False(state.IsLookupInProgress);
    }

    [Fact]
    public async Task ClearAsync_resets_vehicle_and_raises_changed()
    {
        var service = new FakeVehicleLookupService();
        var navigation = new TestNavigationManager();
        var state = new VehicleLookupState(service, navigation);

        state.Vehicle.Details = new DetailsModel { RegistrationNumber = "AB12CDE" };

        var changedCount = 0;
        state.Changed += () => changedCount++;

        await state.ClearAsync();

        Assert.Null(state.Vehicle.Details);
        Assert.Equal("/", navigation.LastUri);
        Assert.True(changedCount > 0);
        Assert.True(state.ChangeVersion > 0);
    }

    [Fact]
    public async Task IsSearching_is_scoped_by_metadata_for_mot_ai_lookups()
    {
        var service = new FakeVehicleLookupService
        {
            AiDelay = TimeSpan.FromMilliseconds(50),
        };
        var navigation = new TestNavigationManager();
        var state = new VehicleLookupState(service, navigation);

        const string metaDataA = """{"TestNumber":1}""";
        const string metaDataB = """{"TestNumber":2}""";

        var lookupTask = state.StartLookupAsync("AB12CDE", VehicleLookupType.AiMotSummary, metaDataA);

        Assert.True(state.IsSearching(VehicleLookupType.AiMotSummary, metaDataA));
        Assert.False(state.IsSearching(VehicleLookupType.AiMotSummary, metaDataB));
        Assert.False(state.IsSearching(VehicleLookupType.AiMotPriceEstimate, metaDataA));

        await lookupTask;

        Assert.False(state.IsSearching(VehicleLookupType.AiMotSummary, metaDataA));
    }

    [Fact]
    public async Task IsSearching_price_estimate_is_scoped_by_metadata()
    {
        var service = new FakeVehicleLookupService
        {
            AiDelay = TimeSpan.FromMilliseconds(50),
        };
        var navigation = new TestNavigationManager();
        var state = new VehicleLookupState(service, navigation);

        const string metaData = """{"TestNumber":5,"DefectIds":[]}""";

        var lookupTask = state.StartLookupAsync("AB12CDE", VehicleLookupType.AiMotPriceEstimate, metaData);

        Assert.True(state.IsSearching(VehicleLookupType.AiMotPriceEstimate, metaData));
        Assert.False(state.IsSearching(VehicleLookupType.AiMotPriceEstimate, """{"TestNumber":6,"DefectIds":[]}"""));

        await lookupTask;

        Assert.False(state.IsSearching(VehicleLookupType.AiMotPriceEstimate, metaData));
    }

    private sealed class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager() => Initialize("https://localhost/", "https://localhost/");

        public string? LastUri { get; private set; }

        protected override void NavigateToCore(string uri, bool forceLoad) => LastUri = uri;
    }

    private sealed class FakeVehicleLookupService : IVehicleLookupService
    {
        public TimeSpan ImagesDelay { get; init; }
        public TimeSpan AiDelay { get; init; }

        public ValueTask<DetailsModel?> GetVehicleDetailsAsync(string registrationNumber) =>
            ValueTask.FromResult<DetailsModel?>(null);

        public ValueTask<IEnumerable<MotTestModel>?> GetMotTestsAsync(string registrationNumber) =>
            ValueTask.FromResult<IEnumerable<MotTestModel>?>([]);

        public async ValueTask<IEnumerable<ImageModel>?> GetVehicleImagesAsync(string registrationNumber)
        {
            if (ImagesDelay > TimeSpan.Zero)
            {
                await Task.Delay(ImagesDelay);
            }

            return [];
        }

        public async ValueTask<AiDataModel?> GetVehicleAiDataAsync(
            string registrationNumber,
            AiType type,
            string metaData)
        {
            if (AiDelay > TimeSpan.Zero)
            {
                await Task.Delay(AiDelay);
            }

            return new AiDataModel { Content = "summary" };
        }

        public ValueTask<int?> GetVehicleLookupCountAsync(string registrationNumber) =>
            ValueTask.FromResult<int?>(0);

        public ValueTask<IEnumerable<LookupModel>?> GetRecentVehicleLookupsAsync() =>
            ValueTask.FromResult<IEnumerable<LookupModel>?>([]);

        public ValueTask<IEnumerable<LookupModel>?> GetRecentVehicleLookupsAsync(string registrationNumber) =>
            ValueTask.FromResult<IEnumerable<LookupModel>?>([]);
    }
}
