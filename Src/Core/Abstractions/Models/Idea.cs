using System;
<<<<<<< HEAD
using System.Collections.Generic;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class Idea
{
    public string Id { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateOnly CreationDate { get; set; }

    public string UsersId { get; set; } = null!;

    public string TopicsId { get; set; } = null!;

    public virtual Topic Topics { get; set; } = null!;

    public virtual User Users { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
=======
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models
{
    public class Idea
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UsersId { get; set; }  
        public string TopicsId { get; set; } 

        public virtual User User { get; set; } 
        public virtual Topic Topic { get; set; } 
    }
}
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
