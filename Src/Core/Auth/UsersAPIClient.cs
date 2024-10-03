using System;

namespace cis_api_legacy_integration_phase_2.Src.Core.Auth;

public class UsersAPIClient
{
    private readonly HttpClient _httpClient = new();
    private readonly String LOGIN_ENDPOINT = "http://localhost:8080/api/users/validate"; 

    public async Task<(bool, string userId)> Login(string username, string password)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("login", username);
            _httpClient.DefaultRequestHeaders.Add("password", password);

            var response = await _httpClient.GetAsync("http://localhost:8080/api/users/validate");
            if (!response.IsSuccessStatusCode) return (false, null);
            var userId = await response.Content.ReadAsStringAsync();

            return (true,userId);
        }
        catch (Exception)
        {
            return (false, null);
        }
    }
}
