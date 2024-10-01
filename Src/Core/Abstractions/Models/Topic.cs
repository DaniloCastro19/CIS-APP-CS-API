﻿using System;
using System.Collections.Generic;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class Topic
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreationDate { get; set; }

    public string UsersId { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public virtual User Users { get; set; } = null!;
}
