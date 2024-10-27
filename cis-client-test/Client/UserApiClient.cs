using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
public class UserApiClient
{
    private readonly HttpClient _httpClient;

    public UserApiClient(string baseAddress, string username, string password)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
        
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<HttpResponseMessage> CreateUserAsync(string name, string login, string password)
    {
        var userData = new
        {
            name = name,
            login = login,
            password = password
        };
        
        var jsonContent = new StringContent(JsonSerializer.Serialize(userData), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/users", jsonContent);
        response.EnsureSuccessStatusCode();
        return response;
    }
}