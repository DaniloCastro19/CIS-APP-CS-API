using System;
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
