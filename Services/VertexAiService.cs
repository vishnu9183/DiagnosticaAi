using Google.Apis.Auth.OAuth2;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HealthcareApp.Services
{
    public class VertexAiChatService
    {
        private readonly IHttpClientFactory _clientFactory;

        // Corrected endpoint for Gemini 2.0 Flash model using generateContent API.
        private const string ChatEndpoint =
            "your endpoint";

        // Path to your Google Service Account JSON key file.
        private const string ServiceAccountPath = "ServiceAccount/imposing-timer-448902-k6-c941aa87f8d5.json";

        public VertexAiChatService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Calls Google Cloud Gemini 2.0 Flash to generate a diagnosis based on symptoms.
        /// </summary>
        /// <param name="prompt">User symptoms</param>
        /// <returns>Generated diagnosis from AI</returns>
        public async Task<string> GetDiagnosisAsync(string prompt)
        {
            var token = await GetAccessTokenAsync();

            // Corrected Request Body Format
            var requestBody = new
            {
                contents = new[]
                {
                    new { role = "user", parts = new[] { new { text = prompt } } }
                },
                generationConfig = new
                {
                    temperature = 1.0,
                    maxOutputTokens = 1024,
                    topP = 0.95
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.PostAsync(ChatEndpoint, jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Vertex AI request failed. Status: {response.StatusCode}, Body: {responseBody}");
            }

            // **Fix: Properly Extract Text from Response**
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                return "No diagnosis available.";
            }

            var firstCandidate = candidates[0];

            if (firstCandidate.TryGetProperty("content", out var contentElement))
            {
                if (contentElement.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                {
                    // Extract text from first part
                    return parts[0].GetProperty("text").GetString() ?? "Diagnosis could not be generated.";
                }
            }

            return "Diagnosis could not be generated.";
        }

        /// <summary>
        /// Obtains an OAuth2 Access Token using the service account JSON.
        /// </summary>
        /// <returns>Google OAuth2 Access Token</returns>
        private async Task<string> GetAccessTokenAsync()
        {
            GoogleCredential credential;
            using (var jsonStream = new FileStream(ServiceAccountPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(jsonStream)
                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            }
            return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }
    }
}
