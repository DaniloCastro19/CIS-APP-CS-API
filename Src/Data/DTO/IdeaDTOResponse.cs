using System;

namespace cis_api_legacy_integration_phase_2.Src.Data.DTO;

public class IdeaDTOResponse
{
    public string Id { get; set;}
    public string Title { get; set; }         
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }
    
    public string OwnerID { get; set; }
    public string OwnerLogin { get; set; }
    public string TopicID { get; set; }
    public string TopicName { get; set; }


}
