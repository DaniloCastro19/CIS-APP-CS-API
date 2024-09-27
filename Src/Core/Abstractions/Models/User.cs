using System;
using System.Collections.Generic;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Topic>? Topic { get; set; } = new List<Topic>();
}