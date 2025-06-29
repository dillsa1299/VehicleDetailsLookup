using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory
{
    public partial class MotHistory
    {
        [Parameter]
        public IEnumerable<IMotTestModel>? MotTests { get; set; }
    }
}
