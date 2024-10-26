using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models
{
    public class Vote: IHasUserId
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsPositive { get; set; }
        public string UserId { get; set; }
        public string OwnerLogin { get; set; }

        public string IdeasId { get; set; }
        public string IdeaName { get; set; }

    }
}