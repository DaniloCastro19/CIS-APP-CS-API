using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class IdeaDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, ErrorMessage = "Idea Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;
        
        [Required(ErrorMessage = "Content is required")]
        [StringLength(2000, ErrorMessage = "Content cannot be longer than 2000 characters")]
        public string Content { get; set; } = null!;

    }
}