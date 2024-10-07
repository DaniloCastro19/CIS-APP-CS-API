using System;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IService<T, D> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByID(Guid id);
    Task<T> Create(D entity, string userID);
    Task Update(D entity, string userID, Guid topicID);
    Task Delete(Guid id, string userID);
}
