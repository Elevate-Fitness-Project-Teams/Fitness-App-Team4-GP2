using Google.GenAI;
using SmartCoachService.Domain.Contracts;

namespace SmartCoachService.Infrastructure.Persistence.AI
{
    public class GeminiService : IAiService
    {
        private readonly Client _client;

        public GeminiService(IConfiguration configuration)
        {
            var apiKey = configuration["Gemini:ApiKey"];
            _client = new Client(apiKey : apiKey);
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash-lite",
                contents: prompt);

            if(response == null || string.IsNullOrEmpty(response.Text))
            {
                throw new InvalidOperationException("Failed to generate content from Gemini API.");
            }

            return response.Text;
        }
    }
}
