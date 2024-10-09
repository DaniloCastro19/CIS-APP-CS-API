using System;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IUserService
{
    Task<User> GetUserById(string id);
}
