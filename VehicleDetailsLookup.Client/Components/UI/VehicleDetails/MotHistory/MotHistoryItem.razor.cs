using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory;
public partial class MotHistoryItem
{
    [Inject]
    private IJSRuntime? JsRuntime { get; set; }

    [Parameter]
    public MotTestModel? Mot { get; set; }

    private IEnumerable<MotDefectModel> DangerousDefects =>
        Mot?.Defects.Where(d => d.Type == MotDefectType.Dangerous || d.Dangerous)
        ?? [];

    private IEnumerable<MotDefectModel> MajorDefects =>
        Mot?.Defects.Where(d => d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major)
        ?? [];

    private IEnumerable<MotDefectModel> OtherDefects =>
        Mot?.Defects.Where(d => !(d.Type == MotDefectType.Dangerous || d.Dangerous || d.Type == MotDefectType.Fail || d.Type == MotDefectType.Major))
        ?? [];

    private Size _iconSize = Size.Large;

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
}
