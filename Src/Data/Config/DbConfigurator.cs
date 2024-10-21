using System;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Data.Config;

public class DbConfigurator : IDbConfigurator
{
    private WebApplicationBuilder _builder;

    public DbConfigurator(WebApplicationBuilder builder){
        _builder = builder;
    }
    
    public WebApplicationBuilder CreateContext(string connectionString, string DbType)
    {
        if(DbType.ToLower() == "mysql"){
            _builder.Services.AddDbContext<DataContext>(options =>
             options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));
        }
        return _builder;
    }
}