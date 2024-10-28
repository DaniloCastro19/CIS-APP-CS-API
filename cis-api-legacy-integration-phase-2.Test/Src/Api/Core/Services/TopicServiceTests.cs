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

namespace cis_api_legacy_integration_phase_2.Test.Src.Api.Core.Services
{
    public class TopicServiceTests
    {
        private readonly Mock<ITopicRepository> _topicRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<OwnershipValidator<Topic>> _ownershipValidatorMock;
        private readonly TopicService _topicService;

        public TopicServiceTests()
        {
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _userServiceMock = new Mock<IUserService>();
            _ownershipValidatorMock = new Mock<OwnershipValidator<Topic>>();
            _topicService = new TopicService(
                _topicRepositoryMock.Object,
                _ownershipValidatorMock.Object,
                _userServiceMock.Object);
        }

        [Fact]
        public async Task GetByTitle_ReturnsTopics_WhenFound()
        {
            // Arrange
            var title = "Test Title";
            var expectedTopics = new List<Topic>
            {
                new Topic
            {
                Id = "df515901-81b9-4095-b0c4-183ce0895a39",
                Title = title,
                Description = "Description for Topic 1",
                OwnerLogin = "owner1",
                UserId = "user1",
                CreationDate = DateTime.Now
            },
            new Topic
            {
                Id = "8868fdba-04d5-4e4f-a0ef-5847352b0b78",
                Title = title,
                Description = "Description for Topic 2",
                OwnerLogin = "owner2",
                UserId = "user2",
                CreationDate = DateTime.Now
            }
            };
            _topicRepositoryMock.Setup(repo => repo.GetByTitle(title))
                .ReturnsAsync(expectedTopics);

            // Act
            var result = await _topicService.GetByTitle(title);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, topic => Assert.Equal(title, topic.Title));
        }

        [Fact]
        public async Task CountTopics_ReturnsCorrectCount()
        {
            // Arrange
            var expectedCount = 5;
            _topicRepositoryMock.Setup(repo => repo.CountTopics())
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _topicService.CountTopics();

            // Assert
            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTopics()
        {
            // Arrange
            var expectedTopics = new List<Topic>
            {
                new Topic { Id = "1", Title = "Topic 1", Description = "Description 1" },
                new Topic { Id = "2", Title = "Topic 2", Description = "Description 2" }
            };
            _topicRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(expectedTopics);

            // Act
            var result = await _topicService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedTopics, result);
        }

        [Fact]
        public async Task GetByID_ReturnsTopic_WhenExists()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var expectedTopic = new Topic { Id = topicId.ToString(), Title = "Test Title", Description = "Test Description" };
            _topicRepositoryMock.Setup(repo => repo.GetByID(topicId))
                .ReturnsAsync(expectedTopic);

            // Act
            var result = await _topicService.GetByID(topicId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTopic.Title, result.Title);
        }

        [Fact]
        public async Task Create_InsertsNewTopic()
        {
            // Arrange
            var topicDto = new TopicDTO { Title = "New Topic", Description = "New Description" };
            var userId = "userId";
            var user = new User { Id = userId, Login = "UserLogin" };
            _userServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(user);

            var newTopic = new Topic
            {
                Id = Guid.NewGuid().ToString(),
                Title = topicDto.Title,
                Description = topicDto.Description,
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                OwnerLogin = user.Login
            };

            _topicRepositoryMock.Setup(repo => repo.Insert(It.IsAny<Topic>()))
                .ReturnsAsync(newTopic);

            // Act
            var result = await _topicService.Create(topicDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(topicDto.Title, result.Title);
            Assert.Equal(userId, result.UserId);
        }

/*        [Fact]
        public async Task Update_UpdatesExistingTopic()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topicDto = new TopicDTO { Title = "Updated Title", Description = "Updated Description" };
            var existingTopic = new Topic { Id = topicId.ToString(), Title = "Old Title", Description = "Old Description", UsersId = "userId" };

            // Simulación de la validación de propiedad
            _ownershipValidatorMock.Setup(validator => validator.ValidateOwnership(topicId, "userId", It.IsAny<IRepositoryGeneric<Topic>>()))
                .Returns(Task.CompletedTask);

            _topicRepositoryMock.Setup(repo => repo.GetByID(topicId)).ReturnsAsync(existingTopic);

            // Act
            await _topicService.Update(topicDto, "userId", topicId);

            // Assert
            Assert.Equal(topicDto.Title, existingTopic.Title);
            Assert.Equal(topicDto.Description, existingTopic.Description);
            _topicRepositoryMock.Verify(repo => repo.Update(existingTopic), Times.Once);
        }*/

/*        [Fact]
        public async Task Delete_RemovesTopic_WhenValid()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            _ownershipValidatorMock.Setup(validator => validator.ValidateOwnership(topicId, It.IsAny<string>(), _topicRepositoryMock.Object))
                .Returns(Task.CompletedTask);

            // Act
            await _topicService.Delete(topicId, "userId");

            // Assert
            _topicRepositoryMock.Verify(repo => repo.Delete(topicId), Times.Once);
        }*/

        [Fact]
        public async Task GetByUser_ReturnsTopicsForUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedTopics = new List<Topic>
            {
                new Topic { Id = "1", Title = "User Topic 1", Description = "Description 1" },
                new Topic { Id = "2", Title = "User Topic 2", Description = "Description 2" }
            };

            _topicRepositoryMock.Setup(repo => repo.GetByUser(userId.ToString()))
                .ReturnsAsync(expectedTopics);

            // Act
            var result = await _topicService.GetByUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedTopics, result);
        }
    }
}
