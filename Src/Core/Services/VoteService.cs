using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly OwnershipValidator<Vote> _ownershipValidator;


        public VoteService(IVoteRepository voteRepository, OwnershipValidator<Vote> ownershipValidator)
        {
            _voteRepository = voteRepository;
            _ownershipValidator = ownershipValidator;

        }

        public async Task<IEnumerable<Vote>> GetAll()
        {
            return await _voteRepository.GetAll();
        }

        public async Task<Vote?> GetByID(Guid id)
        {
            return await _voteRepository.GetByID(id);
        }

        public async Task<Vote> Create(bool voteValue, string userID, Guid voteID)
        {
            var ideaToString = voteID.ToString();
            var newVote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                IsPositive = voteValue,
                UsersId = userID,
                IdeasId = ideaToString,
            };
            return await _voteRepository.Insert(newVote);
        }

        public async Task Update(Guid id, bool voteValue, string userId)
        {
            await _ownershipValidator.ValidateOwnership(id, userId, _voteRepository);
            var existingVote = await _voteRepository.GetByID(id);
            existingVote.IsPositive = voteValue;
            await _voteRepository.Update(existingVote);
        }

        public async Task Delete(Guid id, string userId)
        {
            await _ownershipValidator.ValidateOwnership(id, userId, _voteRepository);
            await _voteRepository.Delete(id);
        }

        public async Task<IEnumerable<Vote>> GetVotesByUserId(Guid userId)
        {
            var id = userId.ToString();
            return await _voteRepository.GetVotesByUserId(id);
        }

        public async Task<IEnumerable<Vote>> GetVotesByIdeaId(Guid ideaId)
        {
            var id = ideaId.ToString();
            return await _voteRepository.GetVotesByIdeaId(id);
        }

        public async Task<int> CountPositiveVotesByIdeaId(Guid ideaId)
        {
            var id = ideaId.ToString();
            return await _voteRepository.CountPositiveVotesByIdeaId(id);
        }

        public async Task<int> CountNegativeVotesByIdeaId(Guid ideaId)
        {
            var id = ideaId.ToString();
            return await _voteRepository.CountNegativeVotesByIdeaId(id);
        }
    }
}