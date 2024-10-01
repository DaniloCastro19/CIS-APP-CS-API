using System;
using System.Collections.Generic;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class Vote
{
    public string Id { get; set; } = null!;

    public bool IsPositive { get; set; }

    public string UsersId { get; set; } = null!;

    public string IdeasId { get; set; } = null!;

    public virtual Idea Ideas { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
