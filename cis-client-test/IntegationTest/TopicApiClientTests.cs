using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_client_test.IntegationTest;

public class TopicApiClientTests
{
    private readonly UserApiClient _userApiClient;
    private readonly string _username;
    private readonly string _password;
    private readonly TopicApiClient _apiClient;
    
    public TopicApiClientTests()
    {
        
        var baseAddressApiCis = Constants.BASE_ADDRESS_API_CIS;
        var baseAddressUsers = Constants.BASE_ADDRESS_API_USER;
        var userNameApiUser = Constants.USERNAME_API_USER;
        var passwordApiUsers = Constants.PASSWORD_API_USER;

        _username = $"Login_Name_{Guid.NewGuid()}";
        _password = Guid.NewGuid().ToString();
        var name = $"User_{Guid.NewGuid()}";
        _userApiClient = new UserApiClient(baseAddressUsers, userNameApiUser, passwordApiUsers);
        _apiClient = new TopicApiClient(baseAddressApiCis, _username, _password);

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
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetAllTopics_Connectivity()
    {
        var topics = await _apiClient.GetAllTopicsAsync();
        topics.Should().NotBeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetTopicById_Connectivity()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().NotBeNull();
        
        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetAllTopics_Structure()
    {
        var topics = await _apiClient.GetAllTopicsAsync();
        topics.Should().BeAssignableTo<IEnumerable<Topic>>();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetTopicById_Structure()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().BeAssignableTo<Topic>();
        
        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }
    
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CreateTopic_DataIntegrity()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        createdTopic.Should().NotBeNull();
        createdTopic.Title.Should().Be(topicDto.Title);
        createdTopic.Description.Should().Be(topicDto.Description);


        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetTopicById_DataIntegrity()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Title.Should().Be(topicDto.Title);
        retrievedTopic.Description.Should().Be(topicDto.Description);


        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }


    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_CreateAndGetTopic_Functionality()
    {
        var topicDto = new TopicDTO { Title = "Functional Test Topic", Description = "Testing functionality." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().NotBeNull();
        retrievedTopic.Title.Should().Be(topicDto.Title);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_UpdateTopic_Functionality()
    {
        var topicDto = new TopicDTO { Title = "Original Title", Description = "Original description." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var updatedDto = new TopicDTO { Title = "Updated Title", Description = "Updated description." };
        await _apiClient.UpdateTopicAsync(createdTopic.Id, updatedDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Title.Should().Be(updatedDto.Title);
        retrievedTopic.Description.Should().Be(updatedDto.Description);
        
        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_DeleteTopic_Functionality()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This topic will be deleted." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        await _apiClient.DeleteTopicAsync(createdTopic.Id);
        Func<Task> act = async () => await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        await act.Should().ThrowAsync<HttpRequestException>(); 
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task Test_GetAllTopics_Performance()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        await _apiClient.GetAllTopicsAsync();
        watch.Stop();
        watch.ElapsedMilliseconds.Should().BeLessThan(900);
    }
}
