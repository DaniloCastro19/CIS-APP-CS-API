﻿using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

public class Topic: IHasUserId
{
    public string Id { get; set; }         
    public string Title { get; set; }     
    public string Description { get; set; } 
    public DateTime CreationDate { get; set; } 
    public string UsersId { get; set; }     
    
    public string OwnerLogin { get; set; }
    
    public ICollection<Idea> Ideas { get; set; }
}
