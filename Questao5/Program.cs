using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Questao2.HttpHelpers;
using Questao5;
using Questao5.Application.Handlers;
using Questao5.Domain.CurrentAccounts.Interfaces;
using Questao5.Domain.Idempotencies.Interfaces;
using Questao5.Domain.Idempotencies.Services;
using Questao5.Domain.Movements.Interfaces;
using Questao5.Infrastructure.Extensions;
using Questao5.Infrastructure.Sqlite;
using Questao5.Repository;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var config = provider.GetRequiredService<DatabaseConfig>();
    var connection = new SqliteConnection(config.Name); 
    connection.Open(); 
    return connection;
});

builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();


builder.Services.AddScoped<ICurrentAccountRepository, CurrentAccountRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.ConfigureServices();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.UseApiDoc();
app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


