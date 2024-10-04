using System;
using RestSharp;
using RestSharp.Authenticators;

namespace cis_api_legacy_integration_phase_2.Src.Core.Auth;

public class UsersAPIClient
{ 
    private readonly HttpClient _httpClient = new();


    public async Task<(bool successLogin, string userId)> Login(string username, string password)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("login", username);
        _httpClient.DefaultRequestHeaders.Add("password", password);
        try
        {
            var response = await _httpClient.GetAsync(Constants.LOGIN_ENDPOINT);
            if (!response.IsSuccessStatusCode) return (false, "");
            var userId = await response.Content.ReadAsStringAsync();
            return (true, userId);
        }
        catch (Exception)
        {
            return (false, "");
        }
    }
}
