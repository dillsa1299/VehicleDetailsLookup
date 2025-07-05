using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace VehicleDetailsLookup.Shared.Helpers
{
    public class DataHash
    {
        public static string GenerateHash<TData>(TData data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(dataJson));
            var dataHash = Convert.ToHexString(hashBytes);

            return dataHash;
        }
    }
}
