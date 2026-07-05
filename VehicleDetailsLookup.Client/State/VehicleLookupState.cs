using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.State;

public sealed class VehicleLookupState(IVehicleLookupService vehicleLookupService, NavigationManager navigationManager)
{
    private readonly IVehicleLookupService _vehicleLookupService = vehicleLookupService;
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly HashSet<LookupActivityKey> _activeLookups = [];

    public event Action? Changed;

    public VehicleModel Vehicle { get; private set; } = new();

    public bool IsLookupInProgress => _activeLookups.Count > 0;

    public string? LastLookupRegistrationNumber { get; private set; }

    public int ChangeVersion { get; private set; }

    public bool IsRecentLookupsHidden =>
        IsLookupInProgress || !string.IsNullOrEmpty(Vehicle.Details?.RegistrationNumber);

    public bool IsSearching(VehicleLookupType lookupType, string? metaData = null) =>
        _activeLookups.Contains(new LookupActivityKey(lookupType, metaData ?? string.Empty));

    public async Task StartLookupAsync(string registrationNumber, VehicleLookupType lookupType, string metaData = "")
    {
        LastLookupRegistrationNumber = registrationNumber;
        var key = new LookupActivityKey(lookupType, metaData);
        SetSearching(key, true);

        try
        {
            await ExecuteLookupAsync(registrationNumber, lookupType, metaData);
        }
        finally
        {
            SetSearching(key, false);
        }
    }

    public Task ClearAsync()
    {
        Vehicle = new VehicleModel();
        LastLookupRegistrationNumber = null;
        _activeLookups.Clear();
        _navigationManager.NavigateTo("/", forceLoad: false);
        NotifyChanged();
        return Task.CompletedTask;
    }

    private async Task ExecuteLookupAsync(string registrationNumber, VehicleLookupType lookupType, string metaData)
    {
        switch (lookupType)
        {
            case VehicleLookupType.Details:
                Vehicle.Details = await _vehicleLookupService.GetVehicleDetailsAsync(registrationNumber);

                var url = Vehicle.Details == null
                    ? "/"
                    : $"/{Vehicle.Details.RegistrationNumber}";
                _navigationManager.NavigateTo(url, forceLoad: false);

                if (Vehicle.Details == null)
                {
                    return;
                }

                Vehicle.MotTests = await _vehicleLookupService.GetMotTestsAsync(registrationNumber) ?? [];

                _ = StartLookupAsync(registrationNumber, VehicleLookupType.Images);
                _ = StartLookupAsync(registrationNumber, VehicleLookupType.AiOverview);
                break;

            case VehicleLookupType.MotHistory:
                Vehicle.MotTests = await _vehicleLookupService.GetMotTestsAsync(registrationNumber) ?? [];
                break;

            case VehicleLookupType.Images:
                Vehicle.Images = await _vehicleLookupService.GetVehicleImagesAsync(registrationNumber) ?? [];
                break;

            case VehicleLookupType.AiOverview:
            case VehicleLookupType.AiCommonIssues:
            case VehicleLookupType.AiMotHistorySummary:
            case VehicleLookupType.AiMotSummary:
            case VehicleLookupType.AiMotPriceEstimate:
                await GetAiDataAsync(registrationNumber, lookupType, metaData);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(lookupType), lookupType, null);
        }

        NotifyChanged();
    }

    private async Task GetAiDataAsync(string registrationNumber, VehicleLookupType lookupType, string metaData)
    {
        var aiType = lookupType switch
        {
            VehicleLookupType.AiOverview => AiType.Overview,
            VehicleLookupType.AiCommonIssues => AiType.CommonIssues,
            VehicleLookupType.AiMotHistorySummary => AiType.MotHistorySummary,
            VehicleLookupType.AiMotSummary => AiType.MotTestSummary,
            VehicleLookupType.AiMotPriceEstimate => AiType.MotPriceEstimate,
            _ => throw new ArgumentOutOfRangeException(nameof(lookupType), lookupType, null),
        };

        var aiData = await _vehicleLookupService.GetVehicleAiDataAsync(registrationNumber, aiType, metaData);

        var storageKey = aiType.ToString() + metaData;

        if (aiData == null)
        {
            Vehicle.AiData.Remove(storageKey);
            return;
        }

        Vehicle.AiData[storageKey] = aiData;
    }

    private void SetSearching(LookupActivityKey key, bool isSearching)
    {
        if (isSearching)
        {
            _activeLookups.Add(key);
        }
        else
        {
            _activeLookups.Remove(key);
        }

        NotifyChanged();
    }

    private void NotifyChanged()
    {
        ChangeVersion++;
        Changed?.Invoke();
    }

    private readonly record struct LookupActivityKey(VehicleLookupType LookupType, string MetaData);
}
