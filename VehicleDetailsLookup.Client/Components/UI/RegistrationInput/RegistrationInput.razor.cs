using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.State;

namespace VehicleDetailsLookup.Client.Components.UI.RegistrationInput;

public partial class RegistrationInput
{
    private readonly RegistrationInputModel _registrationInput = new();
    private bool _showError;

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(LookupState.LastLookupRegistrationNumber))
        {
            _registrationInput.Input = LookupState.LastLookupRegistrationNumber;
        }

        if (LookupState.Vehicle.Details == null &&
            !LookupState.IsSearching(VehicleLookupType.Details) &&
            string.IsNullOrEmpty(LookupState.LastLookupRegistrationNumber))
        {
            _registrationInput.Input = string.Empty;
        }
    }

    protected override void OnLookupStateChanged() => OnParametersSet();

    private void HideError() => _showError = false;

    private async Task LookupRegistration()
    {
        if (_registrationInput.Input.Replace(" ", "").Equals(
                LookupState.Vehicle.Details?.RegistrationNumber,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        _showError = false;
        await LookupState.StartLookupAsync(_registrationInput.Input, VehicleLookupType.Details);

        if (!LookupState.IsSearching(VehicleLookupType.Details) &&
            LookupState.Vehicle.Details == null)
        {
            _showError = true;
        }
    }

    private class RegistrationInputModel
    {
        public string Input { get; set; } = string.Empty;
    }
}
