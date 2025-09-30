using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using Tenisu.Api.Common.Extensions;
using Tenisu.Api.Ranking.Player;
using Tenisu.Application;
using Tenisu.Application.Ranking.GetRankedPlayer;
using Tenisu.Application.Ranking.ListRankedPlayers;
using Tenisu.Application.Ranking.RankPlayer;
using Tenisu.Application.Ranking.RetrieveStatistics;
using Tenisu.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.BuildApplication();
builder.BuildInfrastructure();

builder.Services.AddHttpLogging();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

var ranking = app.MapGroup("/api/ranking");

ranking.MapPost("/players", async (RankPlayerPayload payload, RankPlayerUseCase useCase, CancellationToken cancellationToken) =>
    {
        var result = await useCase.HandleAsync(payload.ToUseCaseRequest, cancellationToken);
        
        return result.IsSuccess ? Results.Created() : result.Error.ToHttpResponse();
    })
    .WithName("RankPlayer")
    .WithDescription("Ajouter un joueur au classement.");

ranking.MapGet("/players", async (ListRankedPlayerUseCase useCase, CancellationToken cancellationToken) =>
    {
        var result = await useCase.HandleAsync(cancellationToken);

        return Results.Ok(result.Players);
    })
    .WithName("ListRankedPlayer")
    .WithDescription("Lister les joueurs du classement.");

ranking.MapGet("/players/{id:guid}", async (Guid id, GetRankedPlayerUseCase useCase, CancellationToken cancellationToken) =>
    {
        var result = await useCase.HandleAsync(id, cancellationToken);
        
        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.ToHttpResponse();
    })
    .WithName("GetRankedPlayer")
    .WithDescription("Récupérer les informations d'un joueur.");

ranking.MapGet("/statistics", async (RetrieveStatisticsUseCase useCase, CancellationToken cancellationToken) =>
    {
        var result = await useCase.HandleAsync(cancellationToken);
        
        return Results.Ok(result);
    })
    .WithName("RetrieveStatistics")
    .WithDescription("Consulter les différentes statistiques du classement.");

app.Run();