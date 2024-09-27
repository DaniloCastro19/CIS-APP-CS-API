using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public partial class Topic
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = null!;
    
    public DateOnly? Creation_date { get; set; } = null!;

    [JsonIgnore]
    public string? UserId { get; set; } = null!;

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual User User { get; set; } = null!;

}