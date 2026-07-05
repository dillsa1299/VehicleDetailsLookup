using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.State;

namespace VehicleDetailsLookup.Client.Components.Pages;

public partial class Main : IDisposable
{
    [Inject]
    private VehicleLookupState LookupState { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public string? RegistrationNumberUrlInput { get; set; }

    private string PageTitle => string.IsNullOrEmpty(LookupState.Vehicle.Details?.RegistrationNumber)
        ? "Vehicle Details Lookup"
        : $"{LookupState.Vehicle.Details?.YearOfManufacture} {LookupState.Vehicle.Details?.Make} {LookupState.Vehicle.Details?.Model} | VDL";

    protected override void OnInitialized()
    {
        LookupState.Changed += OnLookupStateChanged;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(RegistrationNumberUrlInput) &&
            OperatingSystem.IsBrowser() &&
            !RegistrationNumberUrlInput.Replace(" ", "").Equals(
                LookupState.Vehicle.Details?.RegistrationNumber,
                StringComparison.InvariantCultureIgnoreCase))
        {
            await LookupState.StartLookupAsync(RegistrationNumberUrlInput, VehicleLookupType.Details);
        }

        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("hideLoader");
        }
    }

    private void OnLookupStateChanged() => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        LookupState.Changed -= OnLookupStateChanged;
    }
}
