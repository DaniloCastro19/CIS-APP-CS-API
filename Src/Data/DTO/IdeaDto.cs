using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class IdeaDTO
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(2000, ErrorMessage = "Content cannot be longer than 2000 characters")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "UsersId is required")]
        public string UsersId { get; set; } = null!;

        [Required(ErrorMessage = "TopicsId is required")]
        public string TopicsId { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public static Idea ToCompleteIdea(IdeaDTO ideaDto)
        {
            return new Idea
            {
                Id = Guid.NewGuid().ToString(),
                Content = ideaDto.Content,
                CreationDate = DateTime.UtcNow, 
                UsersId = ideaDto.UsersId,
                TopicsId = ideaDto.TopicsId
            };
        }
    }
}