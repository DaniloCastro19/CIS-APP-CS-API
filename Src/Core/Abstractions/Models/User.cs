using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

public partial class User
{
    public String Id { get; set; } 
    public string Login { get; set; } 
    public string Name { get; set; } 
    public string Password { get; set; } 

<<<<<<< HEAD
    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
=======
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
    public ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
>>>>>>> 7eb89777f17c4deaf67b66b75d5448588440f5a1
