using System;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace cis_api_legacy_integration_phase_2.Src.Data.Context;

public class DataContext:DbContext
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<TopicModel> Topic { get; set;}

}
