using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class Topic
{
    public string Id { get; set; }         
    public string Title { get; set; }     
    public string Description { get; set; } 
    public DateTime CreationDate { get; set; } 
    public String UsersId { get; set; }     
    
    public User User { get; set; }
}
