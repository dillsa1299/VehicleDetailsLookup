
namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    /// <summary>
    /// Represents an authentication token for the MOT API.
    /// </summary>
    public interface IMotAuthToken
    {
        /// <summary>
        /// Gets or sets the expiration time of the token.
        /// </summary>
        DateTime ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the token string.
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Gets or sets the type of the token (e.g., Bearer).
        /// </summary>
        string Type { get; set; }
    }
}