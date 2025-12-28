using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory;
public partial class MotHistoryItem
{
    [Inject]
    private IJSRuntime? JsRuntime { get; set; }

    [Inject]
    private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

    [Parameter]
    public VehicleModel Vehicle { get; set; } = default!;

    [Parameter]
    public MotTestModel Mot { get; set; } = default!;

    private IEnumerable<MotDefectModel> DangerousDefects =>
        Mot.Defects.Where(d => d.Type == MotDefectType.Dangerous || d.Dangerous)
            ?? [];

    private IEnumerable<MotDefectModel> MajorDefects =>
        Mot.Defects.Where(d => d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major)
            ?? [];

    private IEnumerable<MotDefectModel> OtherDefects =>
        Mot.Defects.Where(d => !(d.Type == MotDefectType.Dangerous || d.Dangerous || d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major))
            ?? [];

    private string SummaryLookupMetaData => JsonSerializer.Serialize(new AiMotTestSummaryMetaDataModel
    {
        TestNumber = Mot.TestNumber,
    });

    private string PriceEstimateLookupMetaData => JsonSerializer.Serialize(new AiMotPriceEstimateMetaDataModel
    {
        TestNumber = Mot.TestNumber,
        DefectIds = _selectedDefectIds.OrderBy(id => id),
    });

    private string? AiMotSummaryText =>
        Vehicle?.AiData.TryGetValue(AiType.MotTestSummary.ToString() + SummaryLookupMetaData, out var aiDataModel) == true
            ? aiDataModel.Content
            : string.Empty;

    private string? AiPriceEstimateText =>
        Vehicle?.AiData.TryGetValue(AiType.MotPriceEstimate.ToString() + PriceEstimateLookupMetaData, out var aiDataModel) == true
            ? aiDataModel.Content
            : string.Empty;

    private MarkupString? AiMotSummaryHtml =>
        string.IsNullOrWhiteSpace(AiMotSummaryText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiMotSummaryText);

    private MarkupString? AiPriceEstimateHtml =>
        string.IsNullOrWhiteSpace(AiPriceEstimateText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiPriceEstimateText);

    private bool PriceEstimateDisabled =>
        !_selectedDefectIds.Any() || _isSearchingPriceEstimate;

    private const string _aiFailedMessage = "Unable to generate AI response. Please try again.";
    private Size _iconSize = Size.Large;
    private bool _isSearchingMotSummary;
    private bool _isSearchingPriceEstimate;
    private bool _hasSearchedPriceEstimate;
    private IEnumerable<Guid> _selectedDefectIds = [];

    private bool IsDefectSelected(MotDefectModel defect) =>
        _selectedDefectIds.Contains(defect.Id);

    private void OnDefectSelected(MotDefectModel defect, bool selected)
    {
        _hasSearchedPriceEstimate = false;

        _selectedDefectIds = selected
            ? _selectedDefectIds.Append(defect.Id)
            : _selectedDefectIds.Where(id => id != defect.Id);
    }

    private async Task OnExpandedAsync(bool expanded)
    {
        if (_isSearchingMotSummary || !string.IsNullOrWhiteSpace(AiMotSummaryText))
            return;

        await StartSummaryLookupAsync();
    }

    private async Task StartSummaryLookupAsync()
    {
        if (Vehicle.Details is null)
            return;

        await VehicleLookupEventsService.NotifyStartVehicleLookup(
            Vehicle.Details.RegistrationNumber,
            VehicleLookupType.AiMotSummary,
            SummaryLookupMetaData
        );
    }

    private async Task StartPriceEstimateLookupAsync()
    {
        if (Vehicle.Details is null)
            return;

        _hasSearchedPriceEstimate = true;

        await VehicleLookupEventsService.NotifyStartVehicleLookup(
            Vehicle.Details.RegistrationNumber,
            VehicleLookupType.AiMotPriceEstimate,
            PriceEstimateLookupMetaData
        );
    }

    private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData)
    {
        if (lookupType == VehicleLookupType.AiMotSummary && string.Equals(metaData,SummaryLookupMetaData))
        {
            _isSearchingMotSummary = lookupStarted;
            StateHasChanged();
            return;
        }

        if (lookupType == VehicleLookupType.AiMotPriceEstimate && string.Equals(metaData, PriceEstimateLookupMetaData))
        {
            _isSearchingPriceEstimate = lookupStarted;
            StateHasChanged();
            return;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && JsRuntime != null)
        {
            var size = await JsRuntime.InvokeAsync<int>("getWindowWidth");

            _iconSize = size switch
            {
                < 600 => Size.Small,
                < 960 => Size.Medium,
                _ => Size.Large,
            };
            StateHasChanged();
        }
    }

    protected override void OnInitialized()
    {
        VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
        base.OnInitialized();
    }

    public void Dispose()
    {
        VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
    }
}
