using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces
{
    public interface IVoteRepository:IRepositoryGeneric<Vote>
    {
        Task<IEnumerable<Vote>> GetVotesByUserId(string userId);
        Task<IEnumerable<Vote>> GetVotesByIdeaId(string ideaId);
        Task<int> CountPositiveVotesByIdeaId(string ideaId);
        Task<int> CountNegativeVotesByIdeaId(string ideaId);
        
    }
}