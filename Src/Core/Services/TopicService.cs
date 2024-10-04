using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class TopicService: ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
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

        public async Task<Topic> Create(Topic entity)
        {
            return await _topicRepository.Insert(entity);
        }

        public async Task Update(Topic entity)
        {
            await _topicRepository.Update(entity);
        }

        public async Task<Topic> Delete(Guid id)
        {
            return await _topicRepository.Delete(id);
        }
    }
}