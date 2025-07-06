using VehicleDetailsLookup.Models.ApiResponses.Gemini;

namespace VehicleDetailsLookup.Services.Api.Gemini
{
    /// <summary>
    /// Defines a contract for interacting with the Gemini AI-powered search API.
    /// </summary>
    public interface IGeminiService
    {
        /// <summary>
        /// Sends a prompt to the Gemini AI service asynchronously and retrieves the AI-generated response.
        /// </summary>
        /// <param name="prompt">The input prompt to send to the Gemini AI service.</param>
        /// <returns>
        /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation,
        /// with an <see cref="IGeminiResponse"/> containing the AI-generated response, or <c>null</c> if no response is available.
        /// </returns>
        ValueTask<GeminiResponseModel?> GetGeminiResponseAsync(string prompt);
    }
}
