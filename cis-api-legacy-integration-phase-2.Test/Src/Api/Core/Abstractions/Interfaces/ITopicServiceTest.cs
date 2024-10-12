using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cis_api_legacy_integration_phase_2.Test.Src.Api.Core.Abstractions.Interfaces
{
    public class ITopicServiceTest
    {
        private readonly Mock<ITopicRepository> _mockTopicRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<OwnershipValidator<Topic>> _mockOwnershipValidator;
        private readonly ITopicService _topicService;

        public ITopicServiceTest()
        {
            _mockTopicRepository = new Mock<ITopicRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockOwnershipValidator = new Mock<OwnershipValidator<Topic>>();
            _topicService = new TopicService(
                _mockTopicRepository.Object,
                _mockOwnershipValidator.Object,
                _mockUserService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTopics()
        {
            // Arrange
            var topics = new List<Topic>
            {
                new Topic { Id = "1", Title = "Topic 1", Description = "Description 1" },
                new Topic { Id = "2", Title = "Topic 2", Description = "Description 2" }
            };

            _mockTopicRepository.Setup(repo => repo.GetAll()).ReturnsAsync(topics);

            // Act
            var result = await _topicService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Topic 1", result.First().Title);
        }

        [Fact]
        public async Task GetByID_ReturnsCorrectTopic()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic { Id = topicId.ToString(), Title = "Topic", Description = "Description" };

            _mockTopicRepository.Setup(repo => repo.GetByID(topicId)).ReturnsAsync(topic);

            // Act
            var result = await _topicService.GetByID(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(topic.Title, result.Title);
        }

        [Fact]
        public async Task Create_ValidTopic_CreatesTopic()
        {
            // Arrange
            var topicDto = new TopicDTO { Title = "New Topic", Description = "Description" };
            var userId = Guid.NewGuid().ToString();
            var newTopic = new Topic { Id = Guid.NewGuid().ToString(), Title = topicDto.Title, Description = topicDto.Description };

            _mockUserService.Setup(x => x.GetUserById(userId)).ReturnsAsync(new User { Login = "testuser" });
            _mockTopicRepository.Setup(repo => repo.Insert(It.IsAny<Topic>())).ReturnsAsync(newTopic);

            // Act
            var result = await _topicService.Create(topicDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(topicDto.Title, result.Title);
        }

        [Fact]
        public async Task GetByTitle_ReturnsTopicsByTitle()
        {
            // Arrange
            var title = "Test Title";
            var topics = new List<Topic>
            {
                new Topic { Id = "1", Title = title, Description = "Description" }
            };

            _mockTopicRepository.Setup(repo => repo.GetByTitle(title)).ReturnsAsync(topics);

            // Act
            var result = await _topicService.GetByTitle(title);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(title, result.First().Title);
        }

        [Fact]
        public async Task GetByUser_ReturnsTopicsByUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var topics = new List<Topic>
            {
                new Topic { Id = "1", Title = "Topic 1", Description = "Description 1" }
            };

            var userIdString = userId.ToString();
            _mockTopicRepository.Setup(repo => repo.GetByUser(userIdString)).ReturnsAsync(topics);

            // Act
            var result = await _topicService.GetByUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CountTopics_ReturnsCountOfTopics()
        {
            // Arrange
            _mockTopicRepository.Setup(repo => repo.CountTopics()).ReturnsAsync(5);

            // Act
            var result = await _topicService.CountTopics();

            // Assert
            Assert.Equal(5, result);
        }
    }
}
