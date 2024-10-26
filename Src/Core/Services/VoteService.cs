using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Validations;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
namespace cis_api_legacy_integration_phase_2.Src.Core.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly OwnershipValidator<Vote> _ownershipValidator;
        private readonly IIdeaService _ideaService;
        private readonly IUserService _userService;


        public VoteService(IVoteRepository voteRepository, OwnershipValidator<Vote> ownershipValidator, IIdeaService ideaService, IUserService userService)
        {
            _voteRepository = voteRepository;
            _ownershipValidator = ownershipValidator;
            _ideaService = ideaService;
            _userService = userService;
        }

        public async Task<IEnumerable<Vote>> GetAll()
        {
            return await _voteRepository.GetAll();
        }

        public async Task<Vote?> GetByID(Guid id)
        {
            return await _voteRepository.GetByID(id);
        }

        public async Task<Vote> Create(bool voteValue, string userID, Guid ideaId)
        {
            var ideaToString = ideaId.ToString();

            User user = await _userService.GetUserById(userID);
            Idea idea = await _ideaService.GetByID(ideaId);
            RepeatedVoteValidator voteValidator = new RepeatedVoteValidator();
            bool alreadyVoted = await voteValidator.ValidateVote(_voteRepository,userID,ideaToString);
            if (alreadyVoted) return null;
            var newVote = new Vote
            {
                Id = Guid.NewGuid().ToString(),
                IsPositive = voteValue,
                UserId = userID,
                OwnerLogin = user.Login,
                IdeasId = ideaToString,
                IdeaName= idea.Title
            
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