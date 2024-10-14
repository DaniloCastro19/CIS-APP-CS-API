namespace cis_api_legacy_integration_phase_2.Test.Api.Core.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Validations;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Moq;
using Xunit;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class IdeaServiceTests
{
    private readonly Mock<IIdeaRepository> _mockRepository;
    private readonly Mock<OwnershipValidator<Idea>> _mockOwnerValidator;
    private readonly Mock<ITopicService> _mockTopicService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly IdeaService _service;

    public IdeaServiceTests()
    {
        _mockRepository = new Mock<IIdeaRepository>();
        _mockOwnerValidator = new Mock<OwnershipValidator<Idea>>();
        _mockTopicService = new Mock<ITopicService>();
        _mockUserService = new Mock<IUserService>();

        _service = new IdeaService(_mockRepository.Object, _mockOwnerValidator.Object, _mockTopicService.Object, _mockUserService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllIdeas()
    {
        var expectedIdeas = new List<Idea> { new Idea(), new Idea() };
        _mockRepository.Setup(repo => repo.GetAll(false)).ReturnsAsync(expectedIdeas);
        var result = await _service.GetAll(false);
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
        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync((Idea)null);
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
    public async Task GetAll_ShouldReturnEmptyList_WhenNoIdeasExist()
    {
        _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Idea>());
        var result = await _service.GetAll(false);
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
    public async Task Create_ShouldReturnNull_WhenTopicDoesNotExist()
    {
        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "Test Content" };
        var userId = "testUser";
        var topicId = Guid.NewGuid();
        _mockTopicService.Setup(service => service.GetByID(topicId)).ReturnsAsync((Topic)null);
        var result = await _service.Create(ideaDto, userId, topicId);
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ShouldNotInsert_WhenTopicIsNull()
    {
        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "Test Content" };
        var topicId = Guid.NewGuid();
        _mockTopicService.Setup(service => service.GetByID(topicId)).ReturnsAsync((Topic)null);
        var result = await _service.Create(ideaDto, "testUserId", topicId);
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.Insert(It.IsAny<Idea>()), Times.Never);
    }

    [Fact]
    public async Task GetByID_ShouldReturnIdea_WhenIdIsValid()
    {
        var ideaId = Guid.NewGuid();
        var expectedIdea = new Idea { Id = ideaId.ToString(), Title = "Test Idea" };
        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync(expectedIdea);
        var result = await _service.GetByID(ideaId);
        Assert.Equal(expectedIdea, result);
    }

    [Fact]
    public async Task GetByID_ShouldReturnNull_WhenIdeaDoesNotExist()
    {
        var ideaId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetByID(ideaId)).ReturnsAsync((Idea)null);
        var result = await _service.GetByID(ideaId);
        Assert.Null(result);
    }
}