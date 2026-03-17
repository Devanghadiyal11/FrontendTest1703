using System.Net;
using System.Text;
using Frontendtest1703.Models;
using Newtonsoft.Json;

namespace Frontendtest1703.Services
{
    public class AuthService
    {

        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }
        public async Task<Login?> AuthenticateUserAsync(string email, string password, string role)
        {
            var requestData = new
            {
                email = email,
                password = password,
                role = role // Ensure role is sent if backend needs it
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Read the error response for details
                var errorContent = await response.Content.ReadAsStringAsync();

                // Log or handle the specific error
                Console.WriteLine($"BadRequest Error: {errorContent}");

                // Typically, you'd parse this as JSON to get specific error messages
                // var errorDetails = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<Login>(json);
                return loginResponse;
            }
            return null;
        }

    }
}
