using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class VoteService
    {
        private readonly IVoteRepository _voteRepository;

        public VoteService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<IEnumerable<Vote>> GetAllVotes()
        {
            return await _voteRepository.GetAll();
        }

        public async Task<Vote?> GetVoteById(Guid id)
        {
            return await _voteRepository.GetByID(id);
        }

        public async Task<Vote> CreateVote(Vote vote)
        {
            return await _voteRepository.Insert(vote);
        }

        public async Task UpdateVote(Vote vote)
        {
            await _voteRepository.Update(vote);
        }

        public async Task<Vote?> DeleteVote(Guid id)
        {
            return await _voteRepository.Delete(id);
        }

        public async Task<int> CountVotes()
        {
            return await _voteRepository.CountVotes();
        }

        public async Task<IEnumerable<Vote>> GetVotesByUserId(string userId)
        {
            return await _voteRepository.GetVotesByUserId(userId);
        }

        public async Task<IEnumerable<Vote>> GetVotesByIdeaId(string ideaId)
        {
            return await _voteRepository.GetVotesByIdeaId(ideaId);
        }

        public async Task<int> CountPositiveVotesByIdeaId(string ideaId)
        {
            return await _voteRepository.CountPositiveVotesByIdeaId(ideaId);
        }

        public async Task<int> CountNegativeVotesByIdeaId(string ideaId)
        {
            return await _voteRepository.CountNegativeVotesByIdeaId(ideaId);
        }
    }
}