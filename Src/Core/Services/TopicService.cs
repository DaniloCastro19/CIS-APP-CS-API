using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Core.Services;

public class TopicService<T> : IRepositoryGeneric<T> where T : class
{
    private readonly DataContext _dataContext;
    private bool _disposed = false;

    public TopicService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    protected DbSet<T> EntitySet
    {
        get
        {
            return _dataContext.Set<T>();
        }
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await EntitySet.ToListAsync();
    }

    public async Task<T> GetByID(Guid id)
    {
        return await EntitySet.FindAsync(id);
    }

    public async Task<T> Insert(T entity)
    {
        EntitySet.Add(entity);
        await Save();
        return entity;

    }

    public async Task Update(T entity)
    {
        var id = (entity as dynamic).Id;
        var existingEntity = await EntitySet.FindAsync(id);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException("Entity not found");
        }

        _dataContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await Save();
    }

    public async Task<T> Delete(Guid id)
    {
        T entity = await EntitySet.FindAsync(id);
        EntitySet.Remove(entity);
        await Save();
        return entity;
    }

    public async Task Save()
    {
        await _dataContext.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}