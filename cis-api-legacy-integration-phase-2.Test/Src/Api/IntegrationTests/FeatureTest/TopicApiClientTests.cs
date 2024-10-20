﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class TopicApiClientTests
{
    private readonly TopicApiClient _apiClient;
    
    public TopicApiClientTests()
    {
        var baseAddress = "http://localhost:5141/";
        var username = "testlogin2";
        var password = "123456789a";

        _apiClient = new TopicApiClient(baseAddress, username, password);
    }
    
    [Fact]
    public async Task Test_GetAllTopics_Connectivity()
    {
        var topics = await _apiClient.GetAllTopicsAsync();
        topics.Should().NotBeNull();
    }

    [Fact]
    public async Task Test_GetTopicById_Connectivity()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().NotBeNull();
        
        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }
    
    [Fact]
    public async Task Test_GetAllTopics_Structure()
    {
        var topics = await _apiClient.GetAllTopicsAsync();
        topics.Should().BeAssignableTo<IEnumerable<Topic>>();
    }

    [Fact]
    public async Task Test_GetTopicById_Structure()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This is a test topic." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().BeAssignableTo<Topic>();
        
        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }
    
    
    [Fact]
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
    public async Task Test_CreateAndGetTopic_Functionality()
    {
        var topicDto = new TopicDTO { Title = "Functional Test Topic", Description = "Testing functionality." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        var retrievedTopic = await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        retrievedTopic.Should().NotBeNull();
        retrievedTopic.Title.Should().Be(topicDto.Title);


        await _apiClient.DeleteTopicAsync(createdTopic.Id);
    }

    [Fact]
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
    public async Task Test_DeleteTopic_Functionality()
    {
        var topicDto = new TopicDTO { Title = "Test Topic", Description = "This topic will be deleted." };
        var createdTopic = await _apiClient.CreateTopicAsync(topicDto);

        await _apiClient.DeleteTopicAsync(createdTopic.Id);
        Func<Task> act = async () => await _apiClient.GetTopicByIdAsync(createdTopic.Id);
        await act.Should().ThrowAsync<HttpRequestException>(); 
    }
    
    [Fact]
    public async Task Test_GetAllTopics_Performance()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        await _apiClient.GetAllTopicsAsync();
        watch.Stop();
        watch.ElapsedMilliseconds.Should().BeLessThan(200);
    }
}
