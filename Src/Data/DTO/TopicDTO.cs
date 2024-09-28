using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System.ComponentModel.DataAnnotations;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class TopicDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
        public string? Creation_date { get; set; }

        public string? UserId { get; set; }

        public static Topic ToCompleteTopic(TopicDTO topicDto)
        {
            return new Topic
            {
                Id = Guid.NewGuid(),
                Title = topicDto.Title,
                Description = topicDto.Description,
                Creation_date = topicDto.Creation_date != null ? DateOnly.Parse(topicDto.Creation_date) : null,
                UserId = topicDto.UserId
            };
        }
    }
}