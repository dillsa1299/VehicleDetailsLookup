﻿using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.TaxStatus
{
    public partial class TaxStatus
    {
        [Parameter]
        public VehicleModel? Vehicle { get; set; }

        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Set _style based on the MOT status
            switch (Vehicle?.TaxStatus?.ToLowerInvariant())
            {
                case "taxed":
                    _style = "background-color: var(--mud-palette-info);";
                    break;
                case "untaxed":
                    _style = "background-color: var(--mud-palette-error);";
                    break;
                case "sorn":
                    _style = "background-color: var(--mud-palette-warning);";
                    break;
                default:
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
            }
        }
    }
}
