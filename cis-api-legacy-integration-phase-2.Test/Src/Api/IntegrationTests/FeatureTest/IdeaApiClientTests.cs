using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class IdeaApiClientTests
{
    private readonly IdeaApiClient _ideaApiClient;
    private readonly TopicApiClient _topicApiClient;
    private readonly UserApiClient _userApiClient;
    private readonly string _username;
    private readonly string _password;
    private Topic _testTopic;

    public IdeaApiClientTests()
    {
        var baseAddressApiCis = "http://localhost:5141/";
        var baseAddressUsers = "http://localhost:4000/";
        _username = $"Login_Name_{Guid.NewGuid()}";
        _password = Guid.NewGuid().ToString();
        var name = $"User_{Guid.NewGuid()}";
        
        _userApiClient = new UserApiClient(baseAddressUsers, "root", "root");
        _ideaApiClient = new IdeaApiClient(baseAddressApiCis, _username, _password);
        _topicApiClient = new TopicApiClient(baseAddressApiCis, _username, _password);
        CreateUserAsync(name, _username, _password).Wait();
    }
    private async Task CreateUserAsync(string name, string login, string password)
    {
        var userId = Guid.NewGuid().ToString();
        
        var response = await _userApiClient.CreateUserAsync(name, login, password);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error when creating the user in the tests");
        }
    }


    public async Task InitializeAsync()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        _testTopic = await _topicApiClient.CreateTopicAsync(topicDto);
    }
    
    public async Task DisposeAsync()
    {
        if (_testTopic != null)
        {
            await _topicApiClient.DeleteTopicAsync(_testTopic.Id);
        }
    }

    [Fact]
    public async Task Test_CreateIdea_DataIntegrity()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "This is a test idea." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        createdIdea.Should().NotBeNull();
        createdIdea.Title.Should().Be(ideaDto.Title);
        createdIdea.Content.Should().Be(ideaDto.Content);

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }

    [Fact]
    public async Task Test_GetIdeaById_Connectivity()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "This is a test idea." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        var retrievedIdea = await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        retrievedIdea.Should().NotBeNull();

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }

    [Fact]
    public async Task Test_GetAllIdeas_Structure()
    {
        await InitializeAsync();

        var ideas = await _ideaApiClient.GetAllIdeasAsync();
        ideas.Should().BeAssignableTo<IEnumerable<Idea>>();

        await DisposeAsync();
    }

    [Fact]
    public async Task Test_GetIdeaById_Structure()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "This is a test idea." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        var retrievedIdea = await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        retrievedIdea.Should().BeAssignableTo<Idea>();

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }

    [Fact]
    public async Task Test_CreateAndGetIdea_Functionality()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Functional Test Idea", Content = "Testing functionality." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);
        var retrievedIdea = await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        retrievedIdea.Should().NotBeNull();
        retrievedIdea.Title.Should().Be(ideaDto.Title);

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }

    [Fact]
    public async Task Test_DeleteIdea_Functionality()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "This idea will be deleted." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        Func<Task> act = async () => await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        await act.Should().ThrowAsync<HttpRequestException>();

        await DisposeAsync();
    }

    [Fact]
    public async Task Test_GetAllIdeas_Performance()
    {
        await InitializeAsync();

        var watch = System.Diagnostics.Stopwatch.StartNew();
        await _ideaApiClient.GetAllIdeasAsync();
        watch.Stop();
        watch.ElapsedMilliseconds.Should().BeLessThan(900);

        await DisposeAsync();
    }

    [Fact]
    public async Task Test_UpdateIdea_DataIntegrity()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Initial Title", Content = "Initial content." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        var updatedDto = new IdeaDTO { Title = "Updated Title", Content = "Updated content." };
        await _ideaApiClient.UpdateIdeaAsync(createdIdea.Id, updatedDto);

        var retrievedIdea = await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        retrievedIdea.Title.Should().Be(updatedDto.Title);
        retrievedIdea.Content.Should().Be(updatedDto.Content);

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }
    
    
    [Fact]
    public async Task Test_UpdateIdea_NoChanges_Functionality()
    {
        await InitializeAsync();

        var ideaDto = new IdeaDTO { Title = "Unchanged Title", Content = "Content that remains the same." };
        var createdIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);

        await _ideaApiClient.UpdateIdeaAsync(createdIdea.Id, ideaDto);

        var retrievedIdea = await _ideaApiClient.GetIdeaByIdAsync(createdIdea.Id);
        retrievedIdea.Title.Should().Be(ideaDto.Title);
        retrievedIdea.Content.Should().Be(ideaDto.Content);

        await _ideaApiClient.DeleteIdeaAsync(createdIdea.Id);
        await DisposeAsync();
    }
    
    [Fact]
    public async Task Test_CreateMultipleIdeas_Performance()
    {
        await InitializeAsync();

        var watch = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < 5; i++)
        {
            var ideaDto = new IdeaDTO { Title = $"Idea {i}", Content = $"Content for idea {i}" };
            await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);
        }
        watch.Stop();
        watch.ElapsedMilliseconds.Should().BeLessThan(1500);

        await DisposeAsync();
    }

}
