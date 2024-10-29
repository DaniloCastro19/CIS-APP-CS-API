using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class VoteApiClientTests
{
    private readonly VoteApiClient _voteApiClient;
    private readonly IdeaApiClient _ideaApiClient;
    private readonly TopicApiClient _topicApiClient;
    private Topic _testTopic;
    private Idea _testIdea;
    private readonly UserApiClient _userApiClient;
    private readonly string _username;
    private readonly string _password;

    public VoteApiClientTests()
    {
        var baseAddress = "http://localhost:5141/";
        var baseAddressUsers = "http://localhost:4000/";

        _username = $"Login_Name_{Guid.NewGuid()}";
        _password = Guid.NewGuid().ToString();
        var name = $"User_{Guid.NewGuid()}";
        
        _userApiClient = new UserApiClient(baseAddressUsers, "root", "root");
        _voteApiClient = new VoteApiClient(baseAddress, _username, _password);
        _ideaApiClient = new IdeaApiClient(baseAddress, _username, _password);
        _topicApiClient = new TopicApiClient(baseAddress, _username, _password);
        
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
        var topicDto = new TopicDTO { Title = "Test Topic for Vote", Description = "This is a test topic for vote." };
        _testTopic = await _topicApiClient.CreateTopicAsync(topicDto);
        
        var ideaDto = new IdeaDTO { Title = "Test Idea for Vote", Content = "This is a test idea for vote." };
        _testIdea = await _ideaApiClient.CreateIdeaAsync(_testTopic.Id, ideaDto);
    }
    
    public async Task DisposeAsync()
    {
        if (_testIdea != null) await _ideaApiClient.DeleteIdeaAsync(_testIdea.Id);
        if (_testTopic != null) await _topicApiClient.DeleteTopicAsync(_testTopic.Id);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CreateVote_DataIntegrity()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        createdVote.Should().NotBeNull();
        createdVote.IdeasId.Should().Be(_testIdea.Id);
        createdVote.IsPositive.Should().BeTrue();

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetVoteById_Connectivity()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        var retrievedVote = await _voteApiClient.GetVoteByIdAsync(createdVote.Id);
        retrievedVote.Should().NotBeNull();
        retrievedVote.Id.Should().Be(createdVote.Id);

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetAllVotes_Structure()
    {
        await InitializeAsync();

        var votes = await _voteApiClient.GetAllVotesAsync();
        votes.Should().BeAssignableTo<IEnumerable<Vote>>();

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetVotesByIdeaId_Structure()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        var votes = await _voteApiClient.GetVotesByIdeaIdAsync(_testIdea.Id);
        votes.Should().ContainSingle(v => v.Id == createdVote.Id);

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CreateAndGetVote_Functionality()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, false);
        var retrievedVote = await _voteApiClient.GetVoteByIdAsync(createdVote.Id);
        
        retrievedVote.Should().NotBeNull();
        retrievedVote.IsPositive.Should().BeFalse();

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_DeleteVote_Functionality()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        await _voteApiClient.DeleteVoteAsync(createdVote.Id);
        Func<Task> act = async () => await _voteApiClient.GetVoteByIdAsync(createdVote.Id);
        await act.Should().ThrowAsync<HttpRequestException>();

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetAllVotes_Performance()
    {
        await InitializeAsync();

        var watch = System.Diagnostics.Stopwatch.StartNew();
        await _voteApiClient.GetAllVotesAsync();
        watch.Stop();
        watch.ElapsedMilliseconds.Should().BeLessThan(900);

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_UpdateVote_DataIntegrity()
    {
        await InitializeAsync();

        var createdVote = await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        await _voteApiClient.UpdateVoteAsync(createdVote.Id, false);

        var updatedVote = await _voteApiClient.GetVoteByIdAsync(createdVote.Id);
        updatedVote.IsPositive.Should().BeFalse();

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CountPositiveVotesByIdeaId_Functionality()
    {
        await InitializeAsync();

        await _voteApiClient.CreateVoteAsync(_testIdea.Id, true);

        var positiveVotes = await _voteApiClient.CountPositiveVotesByIdeaIdAsync(_testIdea.Id);
        positiveVotes.Should().BeGreaterThan(0);

        await DisposeAsync();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CountNegativeVotesByIdeaId_Functionality()
    {
        await InitializeAsync();

        await _voteApiClient.CreateVoteAsync(_testIdea.Id, false);

        var negativeVotes = await _voteApiClient.CountNegativeVotesByIdeaIdAsync(_testIdea.Id);
        negativeVotes.Should().BeGreaterThan(0);

        await DisposeAsync();
    }
}
