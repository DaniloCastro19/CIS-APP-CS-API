using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class TopicService: ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly OwnershipValidator<Topic> _ownershipValidator;


        public TopicService(ITopicRepository topicRepository, OwnershipValidator<Topic> ownershipValidator)
        {
            _topicRepository = topicRepository;
            _ownershipValidator = ownershipValidator;

        }

        public async Task<IEnumerable<Topic>> GetByTitle(string title)
        {
            return await _topicRepository.GetByTitle(title);
        }

        public async Task<int> CountTopics()
        {
            return await _topicRepository.CountTopics();
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await _topicRepository.GetAll();
        }

        public async Task<Topic> GetByID(Guid id)
        {
            return await _topicRepository.GetByID(id);
        }

        public async Task<Topic> Create(TopicDTO entity, string userId)
        {
            var newTopic = new Topic
            {
                Id = Guid.NewGuid().ToString(), 
                Title = entity.Title,
                Description = entity.Description,
                CreationDate = DateTime.UtcNow, 
                UsersId = userId
            };
            return await _topicRepository.Insert(newTopic);
        }

        public async Task Update(TopicDTO entity, string userId, Guid topicId)
        {
            await _ownershipValidator.ValidateOwnership(topicId, userId, _topicRepository);
            
            var existingTopic= await _topicRepository.GetByID(topicId);
            existingTopic.Title = entity.Title;
            existingTopic.Description = entity.Description;
            await _topicRepository.Update(existingTopic);
        }

        public async Task Delete(Guid id)
        {
            await _topicRepository.Delete(id);
        }

        public async Task<IEnumerable<Topic>> GetByUser(Guid userId)
        {
            var id = userId.ToString();
            return await _topicRepository.GetByUser(id);
        }

        public async Task ValidateOwnership(Guid topicId, string userId)
        {
            var topic = await GetByID(topicId);
            if (topic == null)
            {
                throw new KeyNotFoundException("Topic not found.");
            }

            if (topic.UsersId != userId)
            {
                throw new UnauthorizedAccessException("You are not allowed to delete a topic you didn't create.");
            }

        }
    }
}