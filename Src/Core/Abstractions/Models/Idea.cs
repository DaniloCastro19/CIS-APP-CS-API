using System;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models
{
    public class Idea: IHasUserId
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string? Title { get; set; }

        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }  
        public string OwnerLogin { get; set; }

        public string TopicsId { get; set; } 
        public string TopicName { get; set; }

    }
}