using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;

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
        public async Task GetByUser_ExistingUserId_ReturnsOkResult_WithListOfTopics()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockTopics = new List<Topic>
    {
        new Topic { Id = Guid.NewGuid().ToString(), Title = "User Topic 1", Description = "Description 1" },
        new Topic { Id = Guid.NewGuid().ToString(), Title = "User Topic 2", Description = "Description 2" }
    };

            _mockTopicService.Setup(service => service.GetByUser(userId))
                .ReturnsAsync(mockTopics);

            // Act
            var result = await _controller.GetByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Topic>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetByUser_NonExistingUserId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockTopicService.Setup(service => service.GetByUser(userId))
                .ReturnsAsync((IEnumerable<Topic>)null);

            // Act
            var result = await _controller.GetByUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task UpdateTopic_ValidIdAndDTO_ReturnsOkResult()
        {
            var topicId = Guid.NewGuid();
            var topicIdString = topicId.ToString();
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
            _mockTopicService.Setup(service => service.Update(topicDTO, userId, topicId))
                .Returns(Task.CompletedTask);
            var result = await _controller.UpdateTopic(topicIdString, topicDTO);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Topic Updated Succesfully.", okResult.Value);
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
            _mockTopicService.Setup(service => service.Update(It.IsAny<TopicDTO>(), userId, It.IsAny<Guid>()))
                .Throws(new UnauthorizedAccessException("You are not authorized to update this topic"));

            var result = await _controller.UpdateTopic(topicId, topicDTO);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
            Assert.Equal("You are not authorized to update this topic", objectResult.Value.GetType().GetProperty("message").GetValue(objectResult.Value).ToString());
        }



        [Fact]
        public async Task UpdateTopic_TopicNotFound_ReturnsNotFound()
        {
            // Arrange
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
            _mockTopicService.Setup(service => service.Update(topicDTO, userId, It.IsAny<Guid>()))
                .Throws(new KeyNotFoundException());
            var result = await _controller.UpdateTopic(topicId, topicDTO);
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task DeleteTopic_ValidId_ReturnsOk()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _mockTopicService.Setup(service => service.Delete(topicId, userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTopic(topicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Topic Deleted Succesfully.", okResult.Value);
        }


        [Fact]
        public async Task DeleteTopic_UnauthorizedAccess_ReturnsForbidden()
        {
            var topicId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _mockTopicService.Setup(service => service.Delete(topicId, userId))
                .ThrowsAsync(new UnauthorizedAccessException("You are not authorized to update this topic"));
            var result = await _controller.DeleteTopic(topicId);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
            Assert.Equal("You are not authorized to update this topic", objectResult.Value?.GetType().GetProperty("message")?.GetValue(objectResult.Value));
        }


        [Fact]
        public async Task DeleteTopic_TopicNotFound_ReturnsNotFound()
        {
            var topicId = Guid.NewGuid(); 
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _mockTopicService.Setup(service => service.Delete(topicId, userId))
                .ThrowsAsync(new KeyNotFoundException());
            var result = await _controller.DeleteTopic(topicId);
            Assert.IsType<NotFoundResult>(result);
        }

    }
}