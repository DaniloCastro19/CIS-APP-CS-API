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
        // _httpClient.DefaultRequestHeaders.Clear();
        // _httpClient.DefaultRequestHeaders.Add("Username", "root");
        // _httpClient.DefaultRequestHeaders.Add("Password", "root");
        string endpoint = $"{Constants.USER_ENDPOINT}/{userId}";
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;
            string jsonResponse = await response.Content.ReadAsStringAsync();
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            // User user = new User{
            //     Id = userDeserialized.Id,
            //     Login = userDeserialized.Login,
            //     Name = userDeserialized.Name,
            //     Password = userDeserialized.Password,

            // };
            Console.WriteLine(user);
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
