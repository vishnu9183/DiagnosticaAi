using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthcareApp.Services
{
    public class PythonApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PythonApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AnalyzeImageAsync(byte[] fileBytes)
        {
            string base64File = System.Convert.ToBase64String(fileBytes);

            // Build the JSON body with a "file" key.
            var payload = new { file = base64File };
            string json = JsonSerializer.Serialize(payload);

            // Send a POST request to the Flask endpoint.
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            // If Flask is running on port 5000:
            var response = await client.PostAsync("http://127.0.0.1:5001/analyzeFile", content);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}