using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Styling;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotStatus
{
    public partial class MotStatus
    {
        [Parameter]
        public DetailsModel? Details { get; set; }

        private string _statusText = string.Empty;
        private string _dateText = string.Empty;
        private string _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Neutral);

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            switch (Details?.MotStatus)
            {
                case Shared.Models.Enums.MotStatus.Valid:
                    _statusText = "Valid";
                    _dateText = "Expires: " + Details?.MotExpiryDate;
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Success);
                    break;
                case Shared.Models.Enums.MotStatus.Invalid:
                    _statusText = "Invalid";
                    _dateText = "Expired: " + Details?.MotExpiryDate;
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Error);
                    break;
                case Shared.Models.Enums.MotStatus.NoResults:
                    _statusText = "No results found";
                    _dateText = string.Empty;
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Neutral);
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

                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Neutral);
                    break;
            }
        }
    }
}
