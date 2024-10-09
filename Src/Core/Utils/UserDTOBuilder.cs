using System;
using cis_api_legacy_integration_phase_2.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;

namespace cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class UserDTOBuilder : IBuilder<User, UserDto>
{
    public UserDto Build(User entity)
    {
        return new UserDto{
            Login = entity.Login,
        };
    }
}
