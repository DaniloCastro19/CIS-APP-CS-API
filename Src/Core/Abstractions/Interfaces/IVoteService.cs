using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces
{
    public interface IVoteService
    {
        Task<IEnumerable<Vote>> GetVotesByUserId(Guid userId);
        Task<IEnumerable<Vote>> GetVotesByIdeaId(Guid ideaId);
        Task<int> CountPositiveVotesByIdeaId(Guid ideaId);
        Task<int> CountNegativeVotesByIdeaId(Guid ideaId);
        Task<Vote> Create(bool voteValue, string userID, Guid ideaId);
        Task<IEnumerable<Vote>> GetAll();
        Task<Vote?> GetByID(Guid id);
        Task Update(Guid id, bool voteValue, string userId);
        Task Delete(Guid id, string userId);

        
    }
}