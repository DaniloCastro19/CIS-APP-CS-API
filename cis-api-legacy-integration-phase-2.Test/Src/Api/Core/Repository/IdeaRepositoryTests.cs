﻿namespace cis_api_legacy_integration_phase_2.Test.Api.Core.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class IdeaRepositoryTests : IDisposable
{
    private readonly DataContext _context;
    private readonly IdeaRepository _repository;
    private readonly Mock<IVoteRepository> _mockVoteRepository;

    public IdeaRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _mockVoteRepository = new Mock<IVoteRepository>();
        _repository = new IdeaRepository(_context, _mockVoteRepository.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllIdeas()
    {
        var ideas = new List<Idea>
        {
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Idea 1", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" },
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Idea 2", CreationDate = DateTime.Now, UserId = "user2", TopicsId = "topic2", OwnerLogin = "user2", TopicName = "Topic 2" }
        };
        await _context.Set<Idea>().AddRangeAsync(ideas);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAll();

        Assert.Equal(2, result.Count());
        Assert.Equal(ideas.Select(i => i.Title), result.Select(i => i.Title));
    }

    [Fact]
    public async Task GetAll_WithMostWanted_ShouldReturnIdeasOrderedByVotes()
    {
        var ideas = new List<Idea>
        {
            new Idea { Id = "1", Title = "Idea 1", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" },
            new Idea { Id = "2", Title = "Idea 2", CreationDate = DateTime.Now, UserId = "user2", TopicsId = "topic2", OwnerLogin = "user2", TopicName = "Topic 2" }
        };
        await _context.Set<Idea>().AddRangeAsync(ideas);
        await _context.SaveChangesAsync();

        _mockVoteRepository.Setup(m => m.CountPositiveVotesByIdeaId("1")).ReturnsAsync(5);
        _mockVoteRepository.Setup(m => m.CountPositiveVotesByIdeaId("2")).ReturnsAsync(10);

        var result = await _repository.GetAll(mostWanted: true);

        Assert.Equal(2, result.Count());
        Assert.Equal("Idea 2", result.First().Title);
        Assert.Equal("Idea 1", result.Last().Title);
    }

    [Fact]
    public async Task GetByID_ShouldReturnCorrectIdea()
    {
        var ideaId = Guid.NewGuid();
        var idea = new Idea { Id = ideaId.ToString(), Title = "Test Idea", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" };
        await _context.Set<Idea>().AddAsync(idea);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByID(ideaId);

        Assert.NotNull(result);
        Assert.Equal(idea.Title, result.Title);
    }

    [Fact]
    public async Task Insert_ShouldAddNewIdea()
    {
        var newIdea = new Idea { Id = Guid.NewGuid().ToString(), Title = "New Idea", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" };

        var result = await _repository.Insert(newIdea);

        Assert.NotNull(result);
        Assert.Equal(newIdea.Title, result.Title);
        Assert.Equal(1, await _context.Set<Idea>().CountAsync());
    }

    [Fact]
    public async Task Update_ShouldModifyExistingIdea()
    {
        var idea = new Idea { Id = Guid.NewGuid().ToString(), Title = "Original Title", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" };
        await _context.Set<Idea>().AddAsync(idea);
        await _context.SaveChangesAsync();

        idea.Title = "Updated Title";
        await _repository.Update(idea);

        var updatedIdea = await _context.Set<Idea>().FindAsync(idea.Id);
        Assert.NotNull(updatedIdea);
        Assert.Equal("Updated Title", updatedIdea.Title);
    }

    [Fact]
    public async Task Delete_ShouldRemoveIdea()
    {
        var ideaId = Guid.NewGuid();
        var idea = new Idea { Id = ideaId.ToString(), Title = "Idea to Delete", CreationDate = DateTime.Now, UserId = "user1", TopicsId = "topic1", OwnerLogin = "user1", TopicName = "Topic 1" };
        await _context.Set<Idea>().AddAsync(idea);
        await _context.SaveChangesAsync();

        await _repository.Delete(ideaId);

        var deletedIdea = await _context.Set<Idea>().FindAsync(ideaId.ToString());
        Assert.Null(deletedIdea);
    }

    [Fact]
    public async Task CountIdeas_ShouldReturnCorrectCount()
    {
        var userId = "user1";
        var ideas = new List<Idea>
        {
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Idea 1", CreationDate = DateTime.Now, UserId = userId, TopicsId = "topic1", OwnerLogin = userId, TopicName = "Topic 1" },
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Idea 2", CreationDate = DateTime.Now, UserId = userId, TopicsId = "topic2", OwnerLogin = userId, TopicName = "Topic 2" },
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Idea 3", CreationDate = DateTime.Now, UserId = "other_user", TopicsId = "topic3", OwnerLogin = "other_user", TopicName = "Topic 3" }
        };
        await _context.Set<Idea>().AddRangeAsync(ideas);
        await _context.SaveChangesAsync();

        var count = await _repository.CountIdeas(userId);

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetByUser_ShouldReturnUserIdeas()
    {
        var userId = "user1";
        var ideas = new List<Idea>
        {
            new Idea { Id = Guid.NewGuid().ToString(), Title = "User Idea 1", CreationDate = DateTime.Now, UserId = userId, TopicsId = "topic1", OwnerLogin = userId, TopicName = "Topic 1" },
            new Idea { Id = Guid.NewGuid().ToString(), Title = "User Idea 2", CreationDate = DateTime.Now, UserId = userId, TopicsId = "topic2", OwnerLogin = userId, TopicName = "Topic 2" },
            new Idea { Id = Guid.NewGuid().ToString(), Title = "Other Idea", CreationDate = DateTime.Now, UserId = "other_user", TopicsId = "topic3", OwnerLogin = "other_user", TopicName = "Topic 3" }
        };
        await _context.Set<Idea>().AddRangeAsync(ideas);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByUser(userId);

        Assert.Equal(2, result.Count());
        Assert.All(result, idea => Assert.Equal(userId, idea.UserId));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}