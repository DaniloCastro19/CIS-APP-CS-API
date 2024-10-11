using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2;

public class RepeatedVoteValidator
{
    public async Task <bool> ValidateVote(IVoteRepository voteRepository, string userId, string ideaId)
    {

        var votesByUser = await voteRepository.GetVotesByUserId(userId);
        var votesByIdeas = await voteRepository.GetVotesByIdeaId(ideaId);
        List<Vote> relatedVotes= votesByUser.Where(vote => vote.IdeasId == ideaId).ToList();
        return relatedVotes.Count > 0;
    }
}
