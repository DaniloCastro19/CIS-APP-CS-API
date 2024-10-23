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
        _collection = _database.GetCollection<Vote>("votes");
    }
    public async Task<int> CountNegativeVotesByIdeaId(string ideaId)
    {
        int count = (int) await _collection.CountDocumentsAsync(vote => vote.IdeasId == ideaId && vote.IsPositive == false);
        return count;
    }

    public async Task<int> CountPositiveVotesByIdeaId(string ideaId)
    {
       int count = (int) await _collection.CountDocumentsAsync(vote => vote.IdeasId == ideaId && vote.IsPositive == true);
       return count;
    }

    public async Task Delete(Guid id)
    {  
        string idToString = id.ToString();
        await _collection.DeleteOneAsync(vote => vote.Id == idToString);
    }

    public async Task<IEnumerable<Vote>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Vote> GetByID(Guid id)
    {
        var idToString = id.ToString(); 
        var vote = await _collection.Find(vote => vote.Id == idToString).FirstOrDefaultAsync(); 
        return vote;
    }

    public async Task<IEnumerable<Vote>> GetVotesByIdeaId(string ideaId)
    {
        return await _collection.Find(vote => vote.IdeasId == ideaId).ToListAsync(); 
    }

    public async Task<IEnumerable<Vote>> GetVotesByUserId(string userId)
    {
        return await _collection.Find(vote => vote.UsersId == userId).ToListAsync();
    }

    public async Task<Vote> Insert(Vote entity)
    {
        await _collection.InsertOneAsync(entity); 
        return entity;
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public async Task Update(Vote entity)
    {
        await _collection.ReplaceOneAsync(vote => vote.Id == entity.Id, entity);
    }
}
