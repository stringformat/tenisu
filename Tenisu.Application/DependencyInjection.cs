using Microsoft.Extensions.DependencyInjection;
using Tenisu.Application.Ranking.GetRankedPlayer;
using Tenisu.Application.Ranking.ListRankedPlayers;
using Tenisu.Application.Ranking.RankPlayer;
using Tenisu.Application.Ranking.RetrieveStatistics;
using Microsoft.Extensions.Hosting;

namespace Tenisu.Application;

public static class DependencyInjection
{
    public static void BuildApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<RankPlayerUseCase>();
        builder.Services.AddScoped<GetRankedPlayerUseCase>();
        builder.Services.AddScoped<ListRankedPlayerUseCase>();
        builder.Services.AddScoped<RetrieveStatisticsUseCase>();
    }
}