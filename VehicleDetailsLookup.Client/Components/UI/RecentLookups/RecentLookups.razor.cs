using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.State;
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Shared.Models.Lookup;

namespace VehicleDetailsLookup.Client.Components.UI.RecentLookups;

public partial class RecentLookups
{
    [Inject]
    private IVehicleLookupService VehicleLookupService { get; set; } = default!;

    private IEnumerable<LookupModel> _recentLookups = [];
    private bool _isLoading = true;

    private bool IsHidden => LookupState.IsRecentLookupsHidden;

    protected override async Task OnInitializedAsync() =>
        await LoadRecentLookupsIfVisibleAsync();

    protected override void OnLookupStateChanged()
    {
        if (!IsHidden && OperatingSystem.IsBrowser())
        {
            _ = LoadRecentLookupsIfVisibleAsync();
        }
    }

    private async Task HandleLookupClick(string registrationNumber)
    {
        await LookupState.StartLookupAsync(registrationNumber, VehicleLookupType.Details);
    }

    private static string BuildVehicleDetails(LookupModel lookup) =>
        $"{lookup.VehicleDetails.YearOfManufacture} {lookup.VehicleDetails.Make} {lookup.VehicleDetails.Model}";

    private static string GetTimeSpan(DateTime dateTime) =>
        TimeSpanHelper.GetTimeSpan(dateTime, true);

    private async Task LoadRecentLookupsIfVisibleAsync()
    {
        if (IsHidden || !OperatingSystem.IsBrowser())
        {
            return;
        }

        _isLoading = true;
        _recentLookups = await VehicleLookupService.GetRecentVehicleLookupsAsync() ?? [];
        _isLoading = false;
        await InvokeAsync(StateHasChanged);
    }
}
