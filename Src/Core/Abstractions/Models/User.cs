using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

public partial class User
{
    public string Id { get; set; } 
    public string Login { get; set; } 
    public string Name { get; set; } 
    public string Password { get; set; } 
}