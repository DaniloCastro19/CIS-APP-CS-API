using System;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models
{
    public class Idea
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string? Title { get; set; }

        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UsersId { get; set; }  
        public string TopicsId { get; set; } 

        public virtual User User { get; set; } 
        public virtual Topic Topic { get; set; } 
    }
}