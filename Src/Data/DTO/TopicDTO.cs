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

    }
}