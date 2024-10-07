using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IIdeaService
{
    Task<IEnumerable<Idea>> GetAll();
    Task<Idea> GetByID(Guid id);
    Task<Idea> Create(IdeaDTO entity, string userID, Guid topicID);
    Task<Idea> Update(Guid ideaID, IdeaDTO body, string userId);
    Task Delete(Guid id, string userId);
    Task<IEnumerable<Idea>> GetByUser(Guid userId);
    Task<int> CountUserIdeas(Guid userId);
}
