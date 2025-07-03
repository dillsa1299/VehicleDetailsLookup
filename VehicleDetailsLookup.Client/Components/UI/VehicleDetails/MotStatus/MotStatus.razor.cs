using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotStatus
{
    public partial class MotStatus
    {
        [Parameter]
        public IDetailsModel? Details { get; set; }

        private string _statusText = string.Empty;
        private string _dateText = string.Empty;
        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            switch (Details?.MotStatus)
            {
                case Shared.Models.Enums.MotStatus.Valid:
                    _statusText = "Valid";
                    _dateText = "Expires: " + Details?.MotExpiryDate;
                    _style = "background-color: var(--mud-palette-success);";
                    break;
                case Shared.Models.Enums.MotStatus.Invalid:
                    _statusText = "Invalid";
                    _dateText = "Expired: " + Details?.MotExpiryDate;
                    _style = "background-color: var(--mud-palette-error);";
                    break;
                case Shared.Models.Enums.MotStatus.NoResults:
                    _statusText = "No results found";
                    _dateText = string.Empty;
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
                case Shared.Models.Enums.MotStatus.NoDetails:

                    if (Details?.MonthOfFirstRegistration > DateOnly.FromDateTime(DateTime.Now.AddYears(-3)))
                    {
                        _statusText = "MOT not yet due";
                        _dateText = "Due by: " + Details.MonthOfFirstRegistration.AddYears(3);
                    }
                    else
                    {
                        _statusText = "No details held by DVLA";
                        _dateText = string.Empty;
                    }

                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
            }
        }
    }
}
