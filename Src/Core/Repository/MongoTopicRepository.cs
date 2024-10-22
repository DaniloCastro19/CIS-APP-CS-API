using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Data.Config;
using MongoDB.Driver;

namespace cis_api_legacy_integration_phase_2.Src.Core.Repository;

public class MongoTopicRepository : ITopicRepository
{
    protected readonly IMongoDatabase _database;
    protected readonly IMongoCollection<Topic> _collection;

    public MongoTopicRepository(MongoConfig mongoConfig){
        _database = mongoConfig.GetDatabase();
        _collection = _database.GetCollection<Topic>("topics");
    }
    public Task<int> CountTopics()
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Topic>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Topic> GetByID(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Topic>> GetByTitle(string title)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Topic>> GetByUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Topic> Insert(Topic entity)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public Task Update(Topic entity)
    {
        throw new NotImplementedException();
    }
}
