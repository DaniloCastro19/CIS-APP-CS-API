using System;
using System.Text.Json;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using Newtonsoft.Json;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient = new();
    public async Task<User> GetUserById(string userId)
    {
        string endpoint = $"{Constants.USER_ENDPOINT}/{userId}";
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;
            string jsonResponse = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
