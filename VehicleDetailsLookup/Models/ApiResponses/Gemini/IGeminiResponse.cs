namespace VehicleDetailsLookup.Models.ApiResponses.Gemini
{
    public interface IGeminiResponse
    {
        /// <summary>
        /// Response from the Gemini API.
        /// </summary>
        string Response { get; set; }
    }
}