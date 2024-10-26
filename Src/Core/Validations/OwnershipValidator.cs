using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

namespace cis_api_legacy_integration_phase_2.Src.Core.Utils;

public class OwnershipValidator<T> where T:class, IHasUserId
{
    /// <summary>//+
    /// Validates the ownership of an entity based on its ID and the user's ID.//+
    /// </summary>//+
    /// <param name="entityId">The unique identifier of the entity to validate.</param>//+
    /// <param name="userId">The unique identifier of the user performing the validation.</param>//+
    /// <param name="repository">The repository interface for accessing the entity data.</param>//+
    /// <returns>A task that represents the asynchronous operation.</returns>//+
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified ID is not found.</exception>//+
    /// <exception cref="UnauthorizedAccessException">Thrown when the user does not have ownership of the entity.</exception>//
    public async Task ValidateOwnership(Guid entityId, string userId, IRepositoryGeneric<T> repository)
    {
        var entity = await repository.GetByID(entityId);
        if (entity == null)
        {
            throw new KeyNotFoundException("Not found.");
        }

        if (entity.UserId != userId)
        {
            throw new UnauthorizedAccessException("Action not allowed. Please, verify ownership.");
        }

    }
}
