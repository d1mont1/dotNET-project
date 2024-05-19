using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace first_exam.Models
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44383/api/Home/login", new { email, password });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result.Token;
        }

        public async Task RegisterAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44383/api/Home/register", new { email, password });
            response.EnsureSuccessStatusCode();
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }
    }
}
