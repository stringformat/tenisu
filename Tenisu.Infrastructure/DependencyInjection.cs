using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tenisu.Application.Common.DAOs;
using Tenisu.Application.Common.Repositories;
using Tenisu.Common;
using Tenisu.Infrastructure.Common;
using Tenisu.Infrastructure.Ranking;

namespace Tenisu.Infrastructure;

public static class DependencyInjection
{
    public static void BuildInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Tenisu");
        
        builder.Services.AddSqlServer<TenisuContext>(connectionString);
        builder.Services.AddSqlServer<TenisuReadContext>(connectionString);

        builder.Services.AddScoped<IRankingDao, RankingDao>();
        builder.Services.AddScoped<IRankingRepository, RankingRepository>();
        
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}