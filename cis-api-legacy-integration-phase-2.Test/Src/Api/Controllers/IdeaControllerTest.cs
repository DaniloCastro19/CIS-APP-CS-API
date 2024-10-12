namespace cis_api_legacy_integration_phase_2.Test;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

public class IdeaControllerTests
{
    private readonly Mock<IIdeaService> _mockIdeaService;
    private readonly IdeaController _controller;

    public IdeaControllerTests()
    {
        _mockIdeaService = new Mock<IIdeaService>();
        _controller = new IdeaController(_mockIdeaService.Object);
    }

    private void SetupUserContext(string userId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAll_ReturnsOkResultWithIdeas()
    {
        var ideas = new List<Idea> { new Idea(), new Idea() };
        _mockIdeaService.Setup(service => service.GetAll(false)).ReturnsAsync(ideas);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Equal(2, returnedIdeas.Count);
    }

    [Fact]
    public async Task GetAll_EmptyList_ReturnsOkResultWithEmptyList()
    {
        var emptyList = new List<Idea>();
        _mockIdeaService.Setup(service => service.GetAll(false)).ReturnsAsync(emptyList);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Empty(returnedIdeas);
    }

    [Fact]
    public async Task GetAll_WithMostWantedParameter_ReturnsOkResultWithIdeas()
    {
        var ideas = new List<Idea> { new Idea(), new Idea() };
        _mockIdeaService.Setup(service => service.GetAll(true)).ReturnsAsync(ideas);

        var result = await _controller.GetAll(true);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Equal(2, returnedIdeas.Count);
    }

    [Fact]
    public async Task GetByUser_ReturnsOkResultWithIdeas()
    {
        var userId = Guid.NewGuid();
        var ideas = new List<Idea> { new Idea(), new Idea() };
        _mockIdeaService.Setup(service => service.GetByUser(userId)).ReturnsAsync(ideas);

        var result = await _controller.GetByUser(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Equal(2, returnedIdeas.Count);
    }

    [Fact]
    public async Task GetByUser_UserWithNoIdeas_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();
        var emptyList = new List<Idea>();
        _mockIdeaService.Setup(service => service.GetByUser(userId)).ReturnsAsync(emptyList);

        var result = await _controller.GetByUser(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Empty(returnedIdeas);
    }

    [Fact]
    public async Task GetByUser_NonExistingUser_ReturnsOkResultWithEmptyList()
    {
        var userId = Guid.NewGuid();
        var emptyList = new List<Idea>();
        _mockIdeaService.Setup(service => service.GetByUser(userId)).ReturnsAsync(emptyList);

        var result = await _controller.GetByUser(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Empty(returnedIdeas);
    }

    [Fact]
    public async Task GetById_ExistingIdea_ReturnsOkResult()
    {
        var id = Guid.NewGuid();
        var idea = new Idea { Id = id.ToString(), Title = "Test Idea", Content = "Test Content" };
        _mockIdeaService.Setup(service => service.GetByID(id)).ReturnsAsync(idea);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdea = Assert.IsType<Idea>(okResult.Value);
        Assert.Equal(id.ToString(), returnedIdea.Id);
        Assert.Equal("Test Idea", returnedIdea.Title);
        Assert.Equal("Test Content", returnedIdea.Content);
    }

    [Fact]
    public async Task GetById_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        _mockIdeaService.Setup(service => service.GetByID(id)).ReturnsAsync((Idea)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ValidIdea_ReturnsOkResult()
    {
        var topicId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var ideaDto = new IdeaDTO { Title = "New Idea", Content = "New Content" };
        var createdIdea = new Idea { Id = Guid.NewGuid().ToString(), Title = "New Idea", Content = "New Content" };

        SetupUserContext(userId);
        _mockIdeaService.Setup(service => service.Create(ideaDto, userId, topicId)).ReturnsAsync(createdIdea);

        var result = await _controller.Create(topicId, ideaDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedIdea = Assert.IsType<Idea>(okResult.Value);
        Assert.Equal(createdIdea.Id, returnedIdea.Id);
        Assert.Equal(createdIdea.Title, returnedIdea.Title);
        Assert.Equal(createdIdea.Content, returnedIdea.Content);
    }

    [Fact]
    public async Task Update_ValidIdea_ReturnsOkResult()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var ideaDto = new IdeaDTO { Title = "Updated Idea", Content = "Updated Content" };
        var updatedIdea = new Idea { Id = id.ToString(), Title = "Updated Idea", Content = "Updated Content" };

        SetupUserContext(userId);
        _mockIdeaService.Setup(service => service.Update(id, ideaDto, userId)).ReturnsAsync(updatedIdea);

        var result = await _controller.Update(id, ideaDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Idea Updated Succesfully.", okResult.Value);
    }

    [Fact]
    public async Task Update_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var ideaDto = new IdeaDTO { Title = "Test Idea", Content = "This is a test idea." };

        _mockIdeaService.Setup(service => service.Update(id, ideaDto, userId)).Throws(new KeyNotFoundException());
        SetupUserContext(userId);

        var result = await _controller.Update(id, ideaDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_UnauthorizedAccess_ReturnsForbidden()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var ideaDto = new IdeaDTO { Title = "Updated Idea", Content = "Updated Content" };

        SetupUserContext(userId);
        _mockIdeaService.Setup(service => service.Update(id, ideaDto, userId)).ThrowsAsync(new UnauthorizedAccessException());

        var result = await _controller.Update(id, ideaDto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingIdea_ReturnsOkResult()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        _mockIdeaService.Setup(service => service.Delete(id, userId)).Returns(Task.CompletedTask);
        SetupUserContext(userId);

        var result = await _controller.Delete(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Idea Deleted Succesfully.", okResult.Value);
    }

    [Fact]
    public async Task Delete_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        _mockIdeaService.Setup(service => service.Delete(id, userId)).ThrowsAsync(new KeyNotFoundException());
        SetupUserContext(userId);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_UnauthorizedAccess_ReturnsForbidden()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        SetupUserContext(userId);
        _mockIdeaService.Setup(service => service.Delete(id, userId)).ThrowsAsync(new UnauthorizedAccessException());

        var result = await _controller.Delete(id);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
    }
}