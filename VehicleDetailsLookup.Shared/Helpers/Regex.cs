using System.Text.RegularExpressions;

namespace VehicleDetailsLookup.Shared.Helpers
{
    public static partial class RegexHelper
    {
        [GeneratedRegex("^[a-zA-Z0-9]{0,7}$", RegexOptions.CultureInvariant)]
        public static partial Regex RegistrationNumber();
    }
}
