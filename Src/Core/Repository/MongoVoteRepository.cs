using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Config;
using MongoDB.Driver;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository;

public class MongoVoteRepository : IVoteRepository
{
    protected readonly IMongoDatabase _database;
    protected readonly IMongoCollection<Vote> _collection;

    public MongoVoteRepository(MongoConfig mongoConfig){
        _database = mongoConfig.GetDatabase();
        _collection = _database.GetCollection<Vote>("bote");
    }
    public Task<int> CountNegativeVotesByIdeaId(string ideaId)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountPositiveVotesByIdeaId(string ideaId)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Vote>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Vote> GetByID(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Vote>> GetVotesByIdeaId(string ideaId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Vote>> GetVotesByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Vote> Insert(Vote entity)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public Task Update(Vote entity)
    {
        throw new NotImplementedException();
    }
}
