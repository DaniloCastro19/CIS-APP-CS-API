using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Auth;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using cis_api_legacy_integration_phase_2.Src.Core.Validations;

using System;
using cis_api_legacy_integration_phase_2.Src.Core.Utils;
var builder = WebApplication.CreateBuilder(args);

//env variables initialization
DotNetEnv.Env.Load();
var SERVER = Environment.GetEnvironmentVariable("SERVER");
var PORT = Environment.GetEnvironmentVariable("PORT");
var DATABASE = Environment.GetEnvironmentVariable("DATABASE");
var USER = Environment.GetEnvironmentVariable("USER");
var PASSWORD = Environment.GetEnvironmentVariable("PASSWORD");

// DbContext Configuration
var MySqlConnectionString = $"server={SERVER};port={PORT};database={DATABASE};uid={USER};password={PASSWORD};";
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(MySqlConnectionString, new MySqlServerVersion(new Version(8, 0, 21)))); 

// Registry necessary repositories and services
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IIdeaRepository, IdeaRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped(typeof(OwnershipValidator<>));
builder.Services.AddScoped<ITopicService,TopicService>();
builder.Services.AddScoped<IIdeaService, IdeaService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<TopicController>();
builder.Services.AddScoped<IdeaController>();
builder.Services.AddScoped<VoteController>();
// Adding ValidatorsDTO
builder.Services.AddScoped<IValidator<TopicDTO>, TopicDTOValidator>();
builder.Services.AddScoped<IValidator<VoteDto>, VoteDTOValidator>();

// Adding controller service
builder.Services.AddControllers( options =>
{
    options.Filters.Add<ExceptionFilter>();
});
    


// Swagger documentation configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

//Security configuration

builder.Services.AddAuthentication("BasicAuthentication")
.AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication",null);

var app = builder.Build();

// Middlewares configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); 
app.MapControllers(); 
app.Run();