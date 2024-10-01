using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class TopicDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; } 

        public string UsersId { get; set; } = null!;

        public static Topic ToCompleteTopic(TopicDTO topicDto)
        {
            return new Topic
            {
                Id = Guid.NewGuid().ToString(),
                Title = topicDto.Title,
                Description = topicDto.Description,
                CreationDate = topicDto.CreationDate, 
                UsersId = topicDto.UsersId 
            };
        }

    }
}