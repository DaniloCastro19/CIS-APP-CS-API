using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

namespace cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class OwnershipValidator<T> where T:class, IHasUserId
{
    public async Task ValidateOwnership(Guid entityId, string userId, IRepositoryGeneric<T> repository)
    {
        var entity = await repository.GetByID(entityId);
        if (entity == null)
        {
            throw new KeyNotFoundException("Not found.");
        }

        if (entity.UsersId != userId)
        {
            throw new UnauthorizedAccessException("Action not allowed. Please, verify ownership.");
        }

    }
}
