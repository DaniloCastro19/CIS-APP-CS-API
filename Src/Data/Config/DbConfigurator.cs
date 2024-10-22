using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Data.Config;

public class DbConfigurator : IDbConfigurator
{
    private WebApplicationBuilder _builder;

    public DbConfigurator(WebApplicationBuilder builder){
        _builder = builder;
    }
    
    public WebApplicationBuilder CreateContext(string connectionString, string DbType, string mongoDBUri)
    {
        switch(DbType.ToLower())
        {
            case "mysql":
                _builder.Services.AddDbContext<DataContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));
                _builder.Services.AddScoped<ITopicRepository, TopicRepository>();
                _builder.Services.AddScoped<IIdeaRepository, IdeaRepository>();
                _builder.Services.AddScoped<IVoteRepository, VoteRepository>();
                break;
            case "mongo":
                _builder.Services.AddSingleton<MongoConfig>( options => 
                new MongoConfig(mongoDBUri));
                _builder.Services.AddScoped<ITopicRepository, MongoTopicRepository>();
                _builder.Services.AddScoped<IIdeaRepository, MongoIdeaRepository>();
                _builder.Services.AddScoped<IVoteRepository, MongoVoteRepository>();

                break;
            default:
                throw new Exception("Invalid database type. Supported types: MySQL, MongoDB");
        }
        return _builder;
    }
}