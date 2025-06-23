using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Image;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.ImageViewer;
public partial class ImageViewer
{
    [Parameter]
    public bool IsSearching { get; set; }

    [Parameter]
    public IEnumerable<IImageModel> Images { get; set; } = [];

    [Parameter]
    public string? PlaceholderImage { get; set; } = string.Empty;
}
