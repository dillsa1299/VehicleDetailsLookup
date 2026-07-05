// Tests StatusCardCss constants and verifies MOT/tax status components resolve CSS class names
// instead of inline style strings when parameters change.

using System.Reflection;
using VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotStatus;
using VehicleDetailsLookup.Client.Components.UI.VehicleDetails.TaxStatus;
using VehicleDetailsLookup.Client.Styling;
using VehicleDetailsLookup.Shared.Models.Details;
using MotStatusEnum = VehicleDetailsLookup.Shared.Models.Enums.MotStatus;
using TaxStatusEnum = VehicleDetailsLookup.Shared.Models.Enums.TaxStatus;
using Xunit;

namespace VehicleDetailsLookup.Tests.Styling;

public class StatusCardCssTests
{
    [Fact]
    public void Base_includes_text_element_and_status_card()
    {
        Assert.Contains("text-element", StatusCardCss.Base);
        Assert.Contains("status-card", StatusCardCss.Base);
    }

    [Theory]
    [InlineData(StatusCardCss.Success)]
    [InlineData(StatusCardCss.Error)]
    [InlineData(StatusCardCss.Warning)]
    [InlineData(StatusCardCss.Info)]
    [InlineData(StatusCardCss.Neutral)]
    public void Modifier_classes_do_not_contain_css_property_syntax(string modifierClass)
    {
        Assert.DoesNotContain(":", modifierClass);
        Assert.DoesNotContain("background", modifierClass, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Combine_joins_classes_with_spaces()
    {
        var combined = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Success);

        Assert.Equal($"{StatusCardCss.Base} {StatusCardCss.Success}", combined);
    }

    [Theory]
    [InlineData(MotStatusEnum.Valid, StatusCardCss.Success)]
    [InlineData(MotStatusEnum.Invalid, StatusCardCss.Error)]
    [InlineData(MotStatusEnum.NoResults, StatusCardCss.Neutral)]
    public void MotStatus_resolves_expected_modifier_class(MotStatusEnum status, string expectedModifier)
    {
        var component = new MotStatus
        {
            Details = new DetailsModel
            {
                MotStatus = status,
                MotExpiryDate = DateOnly.FromDateTime(DateTime.Today),
            },
        };

        InvokeOnParametersSet(component);

        var statusClass = GetPrivateField<string>(component, "_statusClass");

        Assert.Contains(StatusCardCss.Base, statusClass);
        Assert.Contains(expectedModifier, statusClass);
    }

    [Theory]
    [InlineData(TaxStatusEnum.Taxed, StatusCardCss.Info)]
    [InlineData(TaxStatusEnum.Untaxed, StatusCardCss.Error)]
    [InlineData(TaxStatusEnum.Sorn, StatusCardCss.Warning)]
    public void TaxStatus_resolves_expected_modifier_class(TaxStatusEnum status, string expectedModifier)
    {
        var component = new TaxStatus
        {
            Details = new DetailsModel
            {
                TaxStatus = status,
            },
        };

        InvokeOnParametersSet(component);

        var statusClass = GetPrivateField<string>(component, "_statusClass");

        Assert.Contains(StatusCardCss.Base, statusClass);
        Assert.Contains(expectedModifier, statusClass);
    }

    private static void InvokeOnParametersSet(object component)
    {
        var method = component.GetType().GetMethod(
            "OnParametersSet",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        Assert.NotNull(method);
        method!.Invoke(component, null);
    }

    private static T GetPrivateField<T>(object instance, string fieldName)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        return (T)field!.GetValue(instance)!;
    }
}
