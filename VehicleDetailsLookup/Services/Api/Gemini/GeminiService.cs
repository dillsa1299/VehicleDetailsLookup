namespace VehicleDetailsLookup.Services.Api.Gemini
{
    using Google.GenAI;

    public class GeminiService(IConfiguration configuration) : IGeminiService
    {
        private readonly string _key = configuration["APIs:Gemini:Key"]
                         ?? throw new InvalidOperationException("Gemini API key not found in configuration.");

        public async ValueTask<string?> GetGeminiResponseAsync(string prompt)
        {
            var client = new Client(apiKey: _key);
            var response =  await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash", contents: prompt
            );

            return response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
        }
    }
}
