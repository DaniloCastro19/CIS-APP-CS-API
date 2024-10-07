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

    [Fact]
    public async Task GetAll_ReturnsOkResultWithIdeas()
    {
        var ideas = new List<Idea> { new Idea(), new Idea() };
        _mockIdeaService.Setup(service => service.GetAll()).ReturnsAsync(ideas);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Equal(2, returnedIdeas.Count);
    }

    [Fact]
    public async Task GetAll_EmptyList_ReturnsOkResultWithEmptyList()
    {
        var emptyList = new List<Idea>();
        _mockIdeaService.Setup(service => service.GetAll()).ReturnsAsync(emptyList);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Empty(returnedIdeas);
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
    public async Task GetById_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        _mockIdeaService.Setup(service => service.GetByID(id)).ReturnsAsync((Idea)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var userid = Guid.NewGuid().ToString();
        var ideaDto = new IdeaDTO();
        _mockIdeaService.Setup(service => service.Update(id, ideaDto, userid)).ReturnsAsync((Idea)null);

        var result = await _controller.Update(id, ideaDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Idea not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task Delete_ExistingIdea_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        var userid = Guid.NewGuid().ToString();
        _mockIdeaService.Setup(service => service.Delete(id,userid)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_NonExistingIdea_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var userid = Guid.NewGuid().ToString();
        _mockIdeaService.Setup(service => service.Delete(id,userid)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByUser_WithValidClaims_ReturnsOkResult()
    {
        var userId = Guid.NewGuid();
        var ideas = new List<Idea> { new Idea(), new Idea() };
        _mockIdeaService.Setup(service => service.GetByUser(userId)).ReturnsAsync(ideas);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await _controller.GetByUser(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedIdeas = Assert.IsType<List<Idea>>(okResult.Value);
        Assert.Equal(2, returnedIdeas.Count);
    }
}