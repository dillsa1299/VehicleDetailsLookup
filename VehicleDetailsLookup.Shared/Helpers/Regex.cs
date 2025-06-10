using System.Text.RegularExpressions;

namespace VehicleDetailsLookup.Shared.Helpers
{
    public static class RegexHelper
    {
        public static readonly Regex RegistrationNumber =
            new(@"^[a-zA-Z0-9]{0,7}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}
