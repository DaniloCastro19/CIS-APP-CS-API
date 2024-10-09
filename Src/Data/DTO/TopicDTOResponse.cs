using System;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO;

public class TopicDTOResponse
{
    public string Id { get; set; }         
    public string Title { get; set; }     
    public string Description { get; set; } 
    public DateTime CreationDate { get; set; } 
    public UserDto User { get; set; }
}
