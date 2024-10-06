using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;

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
            // Arrange
            var mockTopics = new List<Topic>
            {
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 1", Description = "Description 1" },
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 2", Description = "Description 2" }
            };

            _mockTopicService.Setup(service => service.GetAll())
                .ReturnsAsync(mockTopics);

            // Act
            var result = await _controller.GetAllTopics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnValue = Assert.IsType<List<Topic>>(okResult.Value); 
            Assert.Equal(2, returnValue.Count); 
        }
    }
}