namespace cis_api_legacy_integration_phase_2.Test.Api.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Moq;
using Xunit;

public class IdeaServiceTests
{
    private readonly Mock<IIdeaRepository> _mockRepository;
    private readonly IdeaService _service;

    public IdeaServiceTests()
    {
        _mockRepository = new Mock<IIdeaRepository>();
        _service = new IdeaService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllIdeas()
    {
        var expectedIdeas = new List<Idea> { new Idea(), new Idea() };
        _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedIdeas);
        var result = await _service.GetAll();
        Assert.Equal(expectedIdeas, result);
    }

    [Fact]
    public async Task GetByID_ShouldReturnIdeaWhenExists()
    {
        var ideaId = Guid.NewGuid();
        var expectedIdea = new Idea { Id = ideaId.ToString() };
        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync(expectedIdea);
        var result = await _service.GetByID(ideaId);
        Assert.Equal(expectedIdea, result);
    }

    [Fact]
    public async Task GetByID_ShouldReturnNullWhenIdeaDoesNotExist()
    {
        var ideaId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetByID(ideaId))!.ReturnsAsync((Idea)null!);
        var result = await _service.GetByID(ideaId);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUser_ShouldReturnUserIdeas()
    {
        var userId = Guid.NewGuid();
        var expectedIdeas = new List<Idea> { new Idea(), new Idea() };
        _mockRepository.Setup(repo => repo.GetByUser(userId.ToString())).ReturnsAsync(expectedIdeas);
        var result = await _service.GetByUser(userId);
        Assert.Equal(expectedIdeas, result);
    }

    [Fact]
    public async Task CountUserIdeas_ShouldReturnCorrectCount()
    {
        var userId = Guid.NewGuid();
        var expectedCount = 5;
        _mockRepository.Setup(repo => repo.CountIdeas(userId.ToString())).ReturnsAsync(expectedCount);
        var result = await _service.CountUserIdeas(userId);
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task Create_ShouldCreateAndReturnNewIdea()
    {
        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "Test Content" };
        var userId = "testUser";
        var topicId = Guid.NewGuid();
        Idea createdIdea = null;

        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Idea>()))
            .Callback<Idea>(idea => createdIdea = idea)
            .ReturnsAsync((Idea idea) => idea);

        var result = await _service.Create(ideaDto, userId, topicId);
        Assert.NotNull(result);
        Assert.Equal(ideaDto.Title, result.Title);
        Assert.Equal(ideaDto.Content, result.Content);
        Assert.Equal(userId, result.UsersId);
        Assert.Equal(topicId.ToString(), result.TopicsId);
        Assert.NotEqual(Guid.Empty.ToString(), result.Id);
    }

    [Fact]
    public async Task Update_ShouldUpdateAndReturnExistingIdea()
    {
        var ideaId = Guid.NewGuid();
        var existingIdea = new Idea { Id = ideaId.ToString(), Title = "Old Title", Content = "Old Content" };
        var updateDto = new IdeaDTO { Title = "Updated Title", Content = "Updated Content" };

        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync(existingIdea);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Idea>())).Returns(Task.CompletedTask);

        var result = await _service.Update(ideaId, updateDto);
        Assert.NotNull(result);
        Assert.Equal(updateDto.Title, result.Title);
        Assert.Equal(updateDto.Content, result.Content);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<Idea>()), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnNullWhenIdeaDoesNotExist()
    {
        var ideaId = Guid.NewGuid();
        var updateDto = new IdeaDTO { Title = "Updated Title", Content = "Updated Content" };

        _mockRepository.Setup(repo => repo.GetByID(ideaId))!.ReturnsAsync((Idea)null!);
        var result = await _service.Update(ideaId, updateDto);
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<Idea>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ShouldCallRepositoryDelete()
    {
        var ideaId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.Delete(ideaId)).Returns(Task.CompletedTask);
        await _service.Delete(ideaId);
        _mockRepository.Verify(repo => repo.Delete(ideaId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoIdeasExist()
    {
        _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Idea>());
        var result = await _service.GetAll();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByUser_ShouldReturnEmptyList_WhenUserHasNoIdeas()
    {
        var userId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetByUser(userId.ToString())).ReturnsAsync(new List<Idea>());
        var result = await _service.GetByUser(userId);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenRepositoryFails()
    {
        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "Test Content" };
        var userId = "testUser";
        var topicId = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Idea>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _service.Create(ideaDto, userId, topicId));
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenRepositoryFails()
    {
        var ideaId = Guid.NewGuid();
        var existingIdea = new Idea { Id = ideaId.ToString(), Title = "Old Title", Content = "Old Content" };
        var updateDto = new IdeaDTO { Title = "Updated Title", Content = "Updated Content" };

        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync(existingIdea);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Idea>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _service.Update(ideaId, updateDto));
    }
}