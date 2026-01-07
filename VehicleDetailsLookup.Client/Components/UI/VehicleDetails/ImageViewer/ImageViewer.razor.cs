using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Image;
using MudBlazor;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.ImageViewer;
public partial class ImageViewer
{
    [Parameter]
    public bool IsSearching { get; set; }

    [Parameter]
    public IEnumerable<ImageModel> Images { get; set; } = [];

    [Parameter]
    public string? PlaceholderImage { get; set; } = string.Empty;

    private MudCarousel<object>? carouselRef;

    private void MovePrevious() => carouselRef?.Previous();
    private void MoveNext() => carouselRef?.Next();
}
