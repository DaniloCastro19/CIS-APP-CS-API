using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class VoteApiClient
{
    private readonly HttpClient _httpClient;

    public VoteApiClient(string baseAddress, string username, string password)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
        // Configura autenticación básica (si aplica)
        var authToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
    }

    // Obtener todos los votos
    public async Task<IEnumerable<Vote>> GetAllVotesAsync()
    {
        var response = await _httpClient.GetAsync("api/votes");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Vote>>();
    }

    // Obtener voto por ID
    public async Task<Vote> GetVoteByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/votes/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Vote>();
    }

    // Crear un nuevo voto
    public async Task<Vote> CreateVoteAsync(string ideaId, bool voteValue)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/votes/ideas/{ideaId}");
        request.Headers.Add("voteValue", voteValue.ToString());
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Vote>();
    }

    // Actualizar un voto existente
    public async Task UpdateVoteAsync(string id, bool isPositive)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/votes/{id}");
        request.Headers.Add("voteValue", isPositive.ToString());
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    // Eliminar un voto
    public async Task DeleteVoteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/votes/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Obtener votos por ID de usuario
    public async Task<IEnumerable<Vote>> GetVotesByUserIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"api/votes/users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Vote>>();
    }

    // Obtener votos por ID de idea
    public async Task<IEnumerable<Vote>> GetVotesByIdeaIdAsync(string ideaId)
    {
        var response = await _httpClient.GetAsync($"api/votes/ideas/{ideaId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Vote>>();
    }

    // Contar votos positivos por ID de idea
    public async Task<int> CountPositiveVotesByIdeaIdAsync(string ideaId)
    {
        var response = await _httpClient.GetAsync($"api/votes/ideas/{ideaId}/positive/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    // Contar votos negativos por ID de idea
    public async Task<int> CountNegativeVotesByIdeaIdAsync(string ideaId)
    {
        var response = await _httpClient.GetAsync($"api/votes/ideas/{ideaId}/negative/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}
