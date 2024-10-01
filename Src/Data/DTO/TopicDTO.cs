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
        public string? CreationDate { get; set; }

        public string UsersId { get; set; }

        public static Topic ToCompleteTopic(TopicDTO TopicDto)
        {
            return new Topic
            {
                Id = Guid.NewGuid().ToString(),
                Title = TopicDto.Title,
                Description = TopicDto.Description,
                CreationDate = TopicDto.CreationDate != null ? DateOnly.Parse(TopicDto.CreationDate) : DateOnly.MinValue,
                UsersId = TopicDto.UsersId
            };
        }
    }
}