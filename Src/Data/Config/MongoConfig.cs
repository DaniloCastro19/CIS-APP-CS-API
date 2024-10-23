using System;
using MongoDB.Driver;

namespace cis_api_legacy_integration_phase_2.Src.Data.Config;

public class MongoConfig
{
    private readonly IMongoDatabase? _database;
    public MongoConfig(string connectionString)
    {
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(mongoUrl.DatabaseName); 
    }

    public IMongoDatabase GetDatabase() => _database;
}
