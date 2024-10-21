using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Data.Config;

public interface IDbConfigurator
{
    WebApplicationBuilder CreateContext(string connectionString, string DbType); 
}
