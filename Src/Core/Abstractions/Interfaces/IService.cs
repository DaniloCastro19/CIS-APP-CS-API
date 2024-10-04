using System;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IService<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByID(Guid id);
    Task<T> Create(T entity);
    Task Update(T entity);
    Task<T> Delete(Guid id);
}
