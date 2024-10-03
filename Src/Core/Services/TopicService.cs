using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class TopicService
    {
        private readonly ITopicRepository _TopicRepository;

        public TopicService(ITopicRepository TopicRepository)
        {
            _TopicRepository = TopicRepository;
        }

        public async Task<IEnumerable<Topic>> GetAllTopics()
        {
            return await _TopicRepository.GetAll();
        }

<<<<<<< HEAD
        public async Task<Topic> GetTopicById(string id)
=======
        public async Task<Topic?> GetTopicById(Guid id)
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
        {
            return await _TopicRepository.GetByID(id);
        }

        public async Task<Topic> CreateTopic(Topic Topic)
        {
            return await _TopicRepository.Insert(Topic);
        }

        public async Task UpdateTopic(Topic Topic)
        {
            await _TopicRepository.Update(Topic);
        }

        public async Task<Topic> DeleteTopic(string id)
        {
            return await _TopicRepository.Delete(id);
        }
    }
}