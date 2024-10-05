using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using FluentValidation;

namespace cis_api_legacy_integration_phase_2.Src.Core.Validations
{
    public class TopicDTOValidator: AbstractValidator<TopicDTO>
    {
        public TopicDTOValidator() 
        {
            RuleFor(topic => topic.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters");

            RuleFor(x => x.Description)
                .NotNull().NotEmpty()
                .WithMessage("Description cannot be null or empty");
        }
    }
}