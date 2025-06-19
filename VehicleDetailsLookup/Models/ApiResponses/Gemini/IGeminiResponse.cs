namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    /// <summary>
    /// Represents a response from the Gemini API.
    /// </summary>
    public interface IGeminiResponse
    {
        /// <summary>
        /// Response from the Gemini API.
        /// </summary>
        string Response { get; set; }
    }
}