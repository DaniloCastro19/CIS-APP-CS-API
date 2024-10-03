using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class Topic
{
<<<<<<< HEAD
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreationDate { get; set; }

    public string UsersId { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public virtual User Users { get; set; } = null!;
=======
    public string Id { get; set; }         
    public string Title { get; set; }     
    public string Description { get; set; } 
    public DateTime CreationDate { get; set; } 
    public String UsersId { get; set; }     
    
    public User User { get; set; }
    
    public ICollection<Idea> Ideas { get; set; } = new List<Idea>();
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
}
