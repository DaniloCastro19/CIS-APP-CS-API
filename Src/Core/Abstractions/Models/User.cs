using System;
using System.Collections.Generic;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
