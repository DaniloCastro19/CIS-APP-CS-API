using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class TopicApiClient
{
    private readonly HttpClient _httpClient;

    public TopicApiClient(string baseAddress, string username, string password)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };

        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
    {
        var response = await _httpClient.GetAsync("api/topics");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<IEnumerable<Topic>>();
    }


    public async Task<Topic> GetTopicByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/topics/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Topic>();
    }


    public async Task<IEnumerable<Topic>> GetByUserAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"api/topics/users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<IEnumerable<Topic>>();
    }


    public async Task<Topic> CreateTopicAsync(TopicDTO topicDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("api/topics", topicDTO);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsAsync<Topic>();
    }


    public async Task UpdateTopicAsync(string id, TopicDTO topicDTO)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/topics/{id}", topicDTO);
        response.EnsureSuccessStatusCode();
    }


    public async Task DeleteTopicAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/topics/{id}");
        response.EnsureSuccessStatusCode();
    }
}
