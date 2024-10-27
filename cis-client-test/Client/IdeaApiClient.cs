using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class IdeaApiClient
{
    private readonly HttpClient _httpClient;

    public IdeaApiClient(string baseAddress, string username, string password)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
        // Configura autenticación básica (si aplica)
        var authToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
    }

    // Obtener todas las ideas
    public async Task<IEnumerable<Idea>> GetAllIdeasAsync(bool mostWanted = false)
    {
        var response = await _httpClient.GetAsync($"api/ideas?mostWanted={mostWanted}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Idea>>();
    }

    // Obtener ideas por ID de usuario
    public async Task<IEnumerable<Idea>> GetIdeasByUserAsync(String userId)
    {
        var response = await _httpClient.GetAsync($"api/ideas/users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Idea>>();
    }

    // Obtener idea por ID
    public async Task<Idea> GetIdeaByIdAsync(String id)
    {
        var response = await _httpClient.GetAsync($"api/ideas/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Idea>();
    }

    // Crear una nueva idea
    public async Task<Idea> CreateIdeaAsync(String topicId, IdeaDTO ideaDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/ideas/{topicId}", ideaDto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Idea>();
    }

    // Actualizar una idea existente
    public async Task UpdateIdeaAsync(String id, IdeaDTO ideaDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/ideas/{id}", ideaDto);
        response.EnsureSuccessStatusCode();
    }

    // Eliminar una idea
    public async Task DeleteIdeaAsync(String id)
    {
        var response = await _httpClient.DeleteAsync($"api/ideas/{id}");
        response.EnsureSuccessStatusCode();
    }
}
