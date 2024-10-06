using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace cis_api_legacy_integration_phase_2.Test
{
    public class TopicControllerTests
    {
        private readonly Mock<ITopicService> _mockTopicService;
        private readonly Mock<IValidator<TopicDTO>> _mockValidator;
        private readonly TopicController _controller;

        public TopicControllerTests()
        {
            _mockTopicService = new Mock<ITopicService>();
            _mockValidator = new Mock<IValidator<TopicDTO>>();
            
            _controller = new TopicController(_mockTopicService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetAllTopics_ReturnsOkResult_WithListOfTopics()
        {
            var mockTopics = new List<Topic>
            {
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 1", Description = "Description 1" },
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 2", Description = "Description 2" }
            };

            _mockTopicService.Setup(service => service.GetAll())
                .ReturnsAsync(mockTopics);

            var result = await _controller.GetAllTopics();

            var okResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnValue = Assert.IsType<List<Topic>>(okResult.Value); 
            Assert.Equal(2, returnValue.Count); 
        }
        
        [Fact]
        public async Task GetTopicById_ExistingId_ReturnsOkResult_WithTopic()
        {
            var topicId = Guid.NewGuid();
            var mockTopic = new Topic { Id = topicId.ToString(), Title = "Topic 1", Description = "Description 1" };

            _mockTopicService.Setup(service => service.GetByID(topicId))
                .ReturnsAsync(mockTopic);

            var result = await _controller.GetTopicById(topicId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnValue = Assert.IsType<Topic>(okResult.Value); 

            Assert.Equal(mockTopic.Id, returnValue.Id); 
            Assert.Equal(mockTopic.Title, returnValue.Title);
            Assert.Equal(mockTopic.Description, returnValue.Description);
        }

        [Fact]
        public async Task GetTopicById_NonExistingId_ReturnsNotFound()
        {
            var topicId = Guid.NewGuid();

            _mockTopicService.Setup(service => service.GetByID(topicId))
                .ReturnsAsync((Topic)null);

            var result = await _controller.GetTopicById(topicId);

            Assert.IsType<NotFoundResult>(result.Result); 
        }

        [Fact]
        public async Task UpdateTopic_ValidIdAndDTO_ReturnsNoContent()
        {
            var topicId = Guid.NewGuid().ToString();
            var topicDTO = new TopicDTO { Title = "Updated Topic", Description = "Updated Description" };
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _mockTopicService.Setup(service => service.ValidateOwnership(It.IsAny<Guid>(), userId))
                .Returns(Task.CompletedTask);
            _mockTopicService.Setup(service => service.Update(topicDTO, userId, topicId))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateTopic(topicId, topicDTO);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTopic_UnauthorizedAccess_ReturnsForbidden()
        {
            var topicId = Guid.NewGuid().ToString();
            var topicDTO = new TopicDTO { Title = "Updated Topic", Description = "Updated Description" };
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _mockTopicService.Setup(service => service.ValidateOwnership(It.IsAny<Guid>(), userId))
                .ThrowsAsync(new UnauthorizedAccessException("User does not own this topic"));

            var result = await _controller.UpdateTopic(topicId, topicDTO);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTopic_TopicNotFound_ReturnsNotFound()
        {
            var topicId = Guid.NewGuid().ToString();
            var topicDTO = new TopicDTO { Title = "Updated Topic", Description = "Updated Description" };
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _mockTopicService.Setup(service => service.ValidateOwnership(It.IsAny<Guid>(), userId))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdateTopic(topicId, topicDTO);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}