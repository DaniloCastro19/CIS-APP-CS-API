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
    private readonly IVoteRepository _voteRepository;


    public MongoIdeaRepository(MongoConfig mongoConfig, IVoteRepository voteRepository){
        _database = mongoConfig.GetDatabase();
        _collection = _database.GetCollection<Idea>("ideas");
        _voteRepository = voteRepository;
    }

    public Task<int> CountIdeas(string id)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id)
    {
        string idToString = id.ToString();
        await _collection.DeleteOneAsync(idea => idea.Id == idToString);
    }

    public async Task<IEnumerable<Idea>> GetAll(bool mostWanted)
    {
        var ideas = await _collection.Find(_ => true).ToListAsync();
        if(mostWanted)
        {
            return ideas.OrderByDescending(
                idea => _voteRepository.CountPositiveVotesByIdeaId(idea.Id).Result
            ).ToList();
        }
        return ideas;
    }

    public async Task<IEnumerable<Idea>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Idea> GetByID(Guid id)
    {
        var idToString = id.ToString(); 
        var idea = await _collection.Find(idea => idea.Id == idToString).FirstOrDefaultAsync(); 
        return idea;
    }

    public async Task<IEnumerable<Idea>> GetByUser(string userId)
    {
        return await _collection.Find(idea => idea.UserId == userId).ToListAsync();
    }

    public async Task<Idea> Insert(Idea entity)
    {
        await _collection.InsertOneAsync(entity); 
        return entity;
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }

    public async Task Update(Idea entity)
    {
        await _collection.ReplaceOneAsync(idea => idea.Id == entity.Id, entity);
    }
}
    