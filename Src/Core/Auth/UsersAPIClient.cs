using System;
using RestSharp;
using RestSharp.Authenticators;

namespace cis_api_legacy_integration_phase_2.Src.Core.Auth;

public class UsersAPIClient
{ 
    private readonly HttpClient _httpClient = new();

    /// <summary>//+
    /// Authenticates a user with the provided username and password.//+
    /// </summary>//+
    /// <param name="username">The username of the user.</param>//+
    /// <param name="password">The password of the user.</param>//+
    /// <returns>//+
    /// A tuple containing two elements://+
    /// - A boolean indicating whether the login was successful.//+
    /// - A string representing the user's ID if the login was successful, otherwise an empty string.//+
    /// </returns>//
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
