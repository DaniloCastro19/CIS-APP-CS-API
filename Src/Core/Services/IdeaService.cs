using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class IdeaService: IIdeaService
    {
        private readonly IIdeaRepository _ideaRepository;

        public IdeaService(IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }

        public async Task<IEnumerable<Idea>> GetAll()
        {
            return await _ideaRepository.GetAll();
        }

        public async Task<Idea?> GetByID(Guid id)
        {
            return await _ideaRepository.GetByID(id);
        }

        public async Task Create(Idea idea)
        {
            
            await _ideaRepository.Insert(idea);
        }

        public async Task<IEnumerable<Idea>> GetByUser(Guid userId)
        {
            string id = userId.ToString();
            return await _ideaRepository.GetByUser(id);
        }

        public async Task<int> CountUserIdeas(Guid userId)
        {
            string id = userId.ToString();
            return  await _ideaRepository.CountIdeas(id);
        }

        public async Task<Idea> Create(IdeaDTO entity, string userID, Guid topicID)
        {
            var topicIdToString = topicID.ToString();
            var newIdea = new Idea
            {
                Id = Guid.NewGuid().ToString(),
                Title= entity.Title,
                Content = entity.Content,
                CreationDate = DateTime.UtcNow,
                UsersId = userID,
                TopicsId = topicIdToString
            };
            return await _ideaRepository.Insert(newIdea);
        }

        public async Task<Idea> Update(Guid ideaID, IdeaDTO entity)
        {
            var existingIdea = await _ideaRepository.GetByID(ideaID); 
            if (existingIdea == null) return null;
            existingIdea.Title = entity.Title;
            existingIdea.Content = entity.Content;
            await _ideaRepository.Update(existingIdea);
            return existingIdea;
        }
        
        public async Task Delete(Guid id)
        {
            await _ideaRepository.Delete(id);
        }
    }
}