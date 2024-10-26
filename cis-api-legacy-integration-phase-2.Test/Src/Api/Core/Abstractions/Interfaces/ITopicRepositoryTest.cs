using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cis_api_legacy_integration_phase_2.Test.Src.Api.Core.Abstractions.Interfaces
{
    public class ITopicRepositoryTest
    {
        private readonly Mock<ITopicRepository> _mockTopicRepository;

        public ITopicRepositoryTest()
        {
            _mockTopicRepository = new Mock<ITopicRepository>();
        }

        [Fact]
        public async Task GetAll_ReturnsListOfTopics()
        {
            // Arrange
            var mockTopics = new List<Topic>
            {
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 1", Description = "Description 1" },
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Topic 2", Description = "Description 2" }
            };

            _mockTopicRepository.Setup(repo => repo.GetAll())
                .ReturnsAsync(mockTopics);

            // Act
            var result = await _mockTopicRepository.Object.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByID_ExistingId_ReturnsTopic()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var mockTopic = new Topic { Id = topicId.ToString(), Title = "Topic 1", Description = "Description 1" };

            _mockTopicRepository.Setup(repo => repo.GetByID(topicId))
                .ReturnsAsync(mockTopic);

            // Act
            var result = await _mockTopicRepository.Object.GetByID(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockTopic.Id, result.Id);
        }

        [Fact]
        public async Task GetByID_NonExistingId_ReturnsNull()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            _mockTopicRepository.Setup(repo => repo.GetByID(topicId))
                .ReturnsAsync((Topic)null);

            // Act
            var result = await _mockTopicRepository.Object.GetByID(topicId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByTitle_ReturnsListOfTopics()
        {
            // Arrange
            var title = "Topic 1";
            var mockTopics = new List<Topic>
            {
                new Topic { Id = Guid.NewGuid().ToString(), Title = title, Description = "Description 1" },
                new Topic { Id = Guid.NewGuid().ToString(), Title = "Another Topic", Description = "Description 2" }
            };

            _mockTopicRepository.Setup(repo => repo.GetByTitle(title))
                .ReturnsAsync(mockTopics.Where(t => t.Title == title));

            // Act
            var result = await _mockTopicRepository.Object.GetByTitle(title);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(title, result.First().Title);
        }

        [Fact]
        public async Task GetByUser_ReturnsListOfTopics()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var mockTopics = new List<Topic>
            {
                new Topic { Id = Guid.NewGuid().ToString(), Title = "User Topic 1", Description = "Description 1", UserId = userId },
                new Topic { Id = Guid.NewGuid().ToString(), Title = "User Topic 2", Description = "Description 2", UserId = userId }
            };

            _mockTopicRepository.Setup(repo => repo.GetByUser(userId))
                .ReturnsAsync(mockTopics);

            // Act
            var result = await _mockTopicRepository.Object.GetByUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CountTopics_ReturnsCorrectCount()
        {
            // Arrange
            int expectedCount = 5;
            _mockTopicRepository.Setup(repo => repo.CountTopics())
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _mockTopicRepository.Object.CountTopics();

            // Assert
            Assert.Equal(expectedCount, result);
        }
    }
}
