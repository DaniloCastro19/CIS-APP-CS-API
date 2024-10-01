using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

public partial class User
{
    public String Id { get; set; } 
    public string Login { get; set; } 
    public string Name { get; set; } 
    public string Password { get; set; } 

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
    public ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
