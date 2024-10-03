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

<<<<<<< HEAD
        public string? Description { get; set; }
        public string? CreationDate { get; set; }

        public string UsersId { get; set; }
=======
        public string Description { get; set; } = null!;

        public DateTime CreationDate { get; set; } 

        public string UsersId { get; set; } = null!;
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1

        public static Topic ToCompleteTopic(TopicDTO TopicDto)
        {
            return new Topic
            {
                Id = Guid.NewGuid().ToString(),
<<<<<<< HEAD
                Title = TopicDto.Title,
                Description = TopicDto.Description,
                CreationDate = TopicDto.CreationDate != null ? DateOnly.Parse(TopicDto.CreationDate) : DateOnly.MinValue,
                UsersId = TopicDto.UsersId
=======
                Title = topicDto.Title,
                Description = topicDto.Description,
                CreationDate = topicDto.CreationDate, 
                UsersId = topicDto.UsersId 
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
            };
        }

    }
}