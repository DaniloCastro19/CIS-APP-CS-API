using System.ComponentModel.DataAnnotations;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO
{
    public class VoteDto
    {
        [Required(ErrorMessage = "Vote type (positive or negative) is required")]
        public bool IsPositive { get; set; }
        
    }
}