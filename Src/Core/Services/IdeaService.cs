using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class IdeaService(IIdeaRepository ideaRepository)
    {
        public async Task<IEnumerable<Idea>> GetAllIdeas()
        {
            return await ideaRepository.GetAll();
        }

        public async Task<Idea?> GetIdeaById(Guid id)
        {
            return await ideaRepository.GetByID(id);
        }

        public async Task CreateIdea(Idea idea)
        {
            await ideaRepository.Insert(idea);
        }

        public async Task UpdateIdea(Idea idea)
        {
            await ideaRepository.Update(idea);
        }

        public async Task<Idea?> DeleteIdea(Guid id)
        {
            return await ideaRepository.Delete(id);
        }
    }
}