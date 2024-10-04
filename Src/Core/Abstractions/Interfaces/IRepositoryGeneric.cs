using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using System;

namespace cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(Guid id);
        Task<T> Insert(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
        Task Save();
    }
}