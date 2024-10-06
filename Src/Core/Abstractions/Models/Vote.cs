using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models
{
    public class Vote
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsPositive { get; set; }
        public string UsersId { get; set; }
        public string IdeasId { get; set; }
        public virtual User User { get; set; }
        public virtual Idea Idea { get; set; }
    }
}