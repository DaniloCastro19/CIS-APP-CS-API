using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Config;
using MongoDB.Driver;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository;

public class MongoIdeaRepository: IIdeaRepository
{
    protected readonly IMongoDatabase _database;
    protected readonly IMongoCollection<Idea> _collection;

    public MongoIdeaRepository(MongoConfig mongoConfig){
        _database = mongoConfig.GetDatabase();
        _collection = _database.GetCollection<Idea>("ideas");
    }

    public Task<int> CountIdeas(string id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Idea>> GetAll(bool mostWanted)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Idea>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Idea> GetByID(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Idea>> GetByUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Idea> Insert(Idea entity)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public Task Update(Idea entity)
    {
        throw new NotImplementedException();
    }
}
