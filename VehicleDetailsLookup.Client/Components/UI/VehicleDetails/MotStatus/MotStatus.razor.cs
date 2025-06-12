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
            switch (Vehicle?.MotStatus?.ToLowerInvariant())
            {
                case "valid":
                    _style = "background-color: var(--mud-palette-success);";
                    break;
                case "not valid":
                    _style = "background-color: var(--mud-palette-error);";
                    break;
                default:
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
            }
        }
    }
}
