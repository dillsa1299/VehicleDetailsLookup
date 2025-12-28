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
        Mot?.Defects.Where(d => d.Type == MotDefectType.Dangerous || d.Dangerous)
        ?? [];

    private IEnumerable<MotDefectModel> MajorDefects =>
        Mot?.Defects.Where(d => d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major)
        ?? [];

    private IEnumerable<MotDefectModel> OtherDefects =>
        Mot?.Defects.Where(d => !(d.Type == MotDefectType.Dangerous || d.Dangerous || d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major))
        ?? [];

    private string? AiMotSummaryText =>
        Vehicle?.AiData.TryGetValue(AiType.MotTestSummary.ToString() + _metaData, out var aiDataModel) == true
                    ? aiDataModel.Content
                    : string.Empty;

    private MarkupString? AiMotSummaryHtml =>
        string.IsNullOrWhiteSpace(AiMotSummaryText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiMotSummaryText);

    private const string _aiFailedMessage = "Unable to generate AI response. Please try again.";
    private Size _iconSize = Size.Large;
    private string _metaData = string.Empty;
    private bool _isSearchingMotSummary;

    private async Task OnExpandedAsync(bool expanded)
    {
        if (_isSearchingMotSummary || !string.IsNullOrWhiteSpace(AiMotSummaryText))
            return;

        await StartSummaryLookup();
    }

    private async Task StartSummaryLookup()
    {
        if (Vehicle.Details is null)
            return;

        await VehicleLookupEventsService.NotifyStartVehicleLookup(
            Vehicle.Details.RegistrationNumber,
            VehicleLookupType.AiMotSummary,
            _metaData
        );
    }

    private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData)
    {
        if (lookupType == VehicleLookupType.AiMotSummary)
        {
            var data = JsonSerializer.Deserialize<AiMotTestSummaryMetaDataModel>(metaData);

            if (data != null && Mot != null && data.TestNumber == Mot.TestNumber)
            {
                _isSearchingMotSummary = lookupStarted;
                StateHasChanged();
            }
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

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _metaData = JsonSerializer.Serialize(new AiMotTestSummaryMetaDataModel
        {
            TestNumber = Mot?.TestNumber,
        });
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
