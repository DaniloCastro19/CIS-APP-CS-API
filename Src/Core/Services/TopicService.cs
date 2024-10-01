using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class TopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IEnumerable<Topic>> GetAllTopics()
        {
            return await _topicRepository.GetAll();
        }

        public async Task<Topic?> GetTopicById(Guid id)
        {
            return await _topicRepository.GetByID(id);
        }

        public async Task<Topic> CreateTopic(Topic topic)
        {
            return await _topicRepository.Insert(topic);
        }

        public async Task UpdateTopic(Topic topic)
        {
            await _topicRepository.Update(topic);
        }

        public async Task<Topic> DeleteTopic(Guid id)
        {
            return await _topicRepository.Delete(id);
        }
    }
}