using System.ComponentModel.DataAnnotations;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class VoteDto
    {
        [Required(ErrorMessage = "Vote type (positive or negative) is required")]
        public bool IsPositive { get; set; }

        [Required(ErrorMessage = "UsersId is required")]
        public string UsersId { get; set; } = null!;

        [Required(ErrorMessage = "IdeasId is required")]
        public string IdeasId { get; set; } = null!;

        public static Vote ToCompleteVote(VoteDto voteDto)
        {
            return new Vote
            {
                Id = Guid.NewGuid().ToString(),
                IsPositive = voteDto.IsPositive,
                UsersId = voteDto.UsersId,
                IdeasId = voteDto.IdeasId
            };
        }
    }
}