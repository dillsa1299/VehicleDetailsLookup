// Base for Blazor components that read VehicleLookupState and must re-render when it changes.

using Microsoft.AspNetCore.Components;

namespace VehicleDetailsLookup.Client.State;

public abstract class VehicleLookupStateComponentBase : ComponentBase, IDisposable
{
    [Inject]
    protected VehicleLookupState LookupState { get; set; } = default!;

    protected override void OnInitialized() =>
        LookupState.Changed += HandleStateChanged;

    private void HandleStateChanged()
    {
        OnLookupStateChanged();
        _ = InvokeAsync(StateHasChanged);
    }

    protected virtual void OnLookupStateChanged()
    {
    }

    public void Dispose() =>
        LookupState.Changed -= HandleStateChanged;
}
