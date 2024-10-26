using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;

namespace cis_api_legacy_integration_phase_2.Test.Src.Api.Core.Repository
{
    public class TopicRepositoryTest
    {
        private  TopicRepository _repository;
        private  DataContext _context;

        public TopicRepositoryTest()
        {
            contextTest();
        }

        private void contextTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _repository = new TopicRepository(_context);
        }

        private List<Topic> GetPredefinedTopics()
        {
            return new List<Topic>
        {
            new Topic
            {
                Id = "df515901-81b9-4095-b0c4-183ce0895a39",
                Title = "Topic 1",
                Description = "Description for Topic 1",
                OwnerLogin = "owner1",
                UserId = "user1",
                CreationDate = DateTime.Now
            },
            new Topic
            {
                Id = "8868fdba-04d5-4e4f-a0ef-5847352b0b78",
                Title = "Topic 2",
                Description = "Description for Topic 2",
                OwnerLogin = "owner2",
                UserId = "user2",
                CreationDate = DateTime.Now
            }
        };
        
        }

        [Fact]
        public async Task GetAll_ReturnsListOfTopics2()
        {
            contextTest();

            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByID_ReturnsTopic_WhenIdIsValid()
        {
           
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var validId = topics.First().Id; 
            string uuidString = validId; 
            Guid uuid = Guid.Parse(uuidString);
            var result = await _repository.GetByID(uuid);
            Assert.NotNull(result);

            Assert.NotNull(result);
            Assert.Equal("Topic 1", result.Title);
        }

        [Fact]
        public async Task Insert_AddsNewTopic()
        {
            var newTopic = new Topic
            {
                Id = "73b18cf4-9020-434b-8061-6822a09517a9",
                Title = "Topic 3",
                Description = "Description for Topic 3",
                OwnerLogin = "owner3",
                UserId = "user3",
                CreationDate = DateTime.Now
            };

            await _repository.Insert(newTopic);
            await _context.SaveChangesAsync();

            var insertedTopic = await _context.Set<Topic>().FindAsync("73b18cf4-9020-434b-8061-6822a09517a9");
            Assert.NotNull(insertedTopic);
            Assert.Equal("Topic 3", insertedTopic.Title);
        }

        [Fact]
        public async Task Update_ChangesExistingTopic()
        {
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var validId = topics.First().Id;
            string uuidString = validId;
            Guid uuid = Guid.Parse(uuidString);
            var topicToUpdate = await _repository.GetByID(uuid);
            topicToUpdate.Description = "Updated Description";

            await _repository.Update(topicToUpdate);
            await _context.SaveChangesAsync();

            var updatedTopic = await _repository.GetByID(uuid);
            Assert.Equal("Updated Description", updatedTopic.Description);
        }

        [Fact]
        public async Task GetByTitle_ReturnsMatchingTopics()
        {
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByTitle("Topic 1");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Topic 1", result.First().Title);
        }

        [Fact]
        public async Task CountTopics_ReturnsCorrectCount()
        {
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var result = await _repository.CountTopics();

            Assert.Equal(2, result);
        }


        [Fact]
        public async Task Delete_RemovesTopic_WhenIdIsValid()
        {
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var validId = topics.First().Id;
            string uuidString = validId;
            Guid uuid = Guid.Parse(uuidString);
            await _repository.Delete(uuid);
            await _context.SaveChangesAsync();

            var deletedTopic = await _repository.GetByID(uuid);
            Assert.Null(deletedTopic);
        }

        [Fact]
        public async Task GetByUser_ReturnsTopicsForUser()
        {
            var topics = GetPredefinedTopics();
            _context.Set<Topic>().AddRange(topics);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByUser("user1");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Topic 1", result.First().Title);
        }
    }
}
