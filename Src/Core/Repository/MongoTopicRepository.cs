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
    public async Task<int> CountTopics()
    {
        int count = (int)await _collection.CountDocumentsAsync(FilterDefinition<Topic>.Empty);
        return count;
    }

    public async Task Delete(Guid id)
    {
        string idToString = id.ToString();
        await _collection.DeleteOneAsync(topic => topic.Id == idToString);
    }

    public async Task<IEnumerable<Topic>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Topic> GetByID(Guid id)
    {
        var idToString = id.ToString(); 
        var topic = await _collection.Find(topic => topic.Id == idToString).FirstOrDefaultAsync(); 
        return topic;
    }

    public async Task<IEnumerable<Topic>> GetByTitle(string title)
    {
        return await _collection.Find(topic => topic.Title == title).ToListAsync();
    }

    public async Task<IEnumerable<Topic>> GetByUser(string userId)
    {
        return await _collection.Find(topic => topic.UserId == userId).ToListAsync();
    }

    public async Task<Topic> Insert(Topic entity)
    {
        await _collection.InsertOneAsync(entity); 
        return entity;
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public async Task Update(Topic entity)
    {
        await _collection.ReplaceOneAsync(topic => topic.Id == entity.Id, entity);
    }
}
