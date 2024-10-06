using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;

namespace cis_api_legacy_integration_phase_2.Src.Core.Validations
{
    public class VoteDTOValidator : AbstractValidator<VoteDto>
    {
        public VoteDTOValidator()
        {
            RuleFor(vote => vote.IsPositive)
                    .NotNull()
                    .WithMessage("Vote type (positive or negative) is required");
        }
    }
}