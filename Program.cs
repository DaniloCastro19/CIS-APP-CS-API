using cis_api_legacy_integration_phase_2.Src.Api.Controllers;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Interfaces;
using cis_api_legacy_integration_phase_2.Src.Core.Abstractions.Models;
using cis_api_legacy_integration_phase_2.Src.Core.Repository;
using cis_api_legacy_integration_phase_2.Src.Core.Services;
using cis_api_legacy_integration_phase_2.Src.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura el DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21)))); 

// Registra los repositorios y servicios necesarios
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IIdeaRepository, IdeaRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<TopicService>();
builder.Services.AddScoped<IdeaService>(); 
builder.Services.AddScoped<VoteService>();
builder.Services.AddScoped<TopicController>();
builder.Services.AddScoped<IdeaController>();
builder.Services.AddScoped<VoteController>();

// Agrega servicios para controladores
builder.Services.AddControllers();

// Configura Swagger para la documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura el middleware para el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); 
app.MapControllers(); 
app.Run();