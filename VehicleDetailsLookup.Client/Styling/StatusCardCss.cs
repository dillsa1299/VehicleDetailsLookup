// CSS class names for MOT and tax status cards. Modifier classes live in components.css
// and use MudBlazor palette variables so light and dark themes stay in sync.

namespace VehicleDetailsLookup.Client.Styling;

public static class StatusCardCss
{
    public const string Base = "text-element status-card";
    public const string Success = "status-card--success";
    public const string Error = "status-card--error";
    public const string Warning = "status-card--warning";
    public const string Info = "status-card--info";
    public const string Neutral = "status-card--neutral";

    public static string Combine(params string[] classes) =>
        string.Join(' ', classes);
}
