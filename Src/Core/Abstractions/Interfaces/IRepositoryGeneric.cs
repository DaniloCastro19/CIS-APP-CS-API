using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;

public interface IRepositoryGeneric<T>: IDisposable where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByID(Guid id);
    Task<T> Insert(T entity);
    Task<T> Delete(Guid id);
    Task Update(T entity);
}