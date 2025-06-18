using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotStatus
{
    public partial class MotStatus
    {
        [Parameter]
        public VehicleModel? Vehicle { get; set; }

        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Set _style based on the MOT status
            _style = (Vehicle?.MotStatus?.ToLowerInvariant()) switch
            {
                "valid" => "background-color: var(--mud-palette-success);",
                "not valid" => "background-color: var(--mud-palette-error);",
                _ => "background-color: var(--mud-palette-gray-default);",
            };
        }
    }
}
