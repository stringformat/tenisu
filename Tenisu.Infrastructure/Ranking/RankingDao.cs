using Microsoft.EntityFrameworkCore;
using Tenisu.Application.Common.DAOs;
using Tenisu.Application.Ranking.GetRankedPlayer;
using Tenisu.Application.Ranking.ListRankedPlayers;
using Tenisu.Application.Ranking.RetrieveStatistics;
using Tenisu.Domain.RankingAggregate.PlayerEntity;
using Tenisu.Infrastructure.Common;

namespace Tenisu.Infrastructure.Ranking;

public class RankingDao(TenisuReadContext context) : DaoBase(context), IRankingDao
{
    public async Task<GetRankedPlayerReadModel?> GetRankedPlayerAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Players
            .Where(p => p.Id == new PlayerId(id))
            .Join(
                context.Ranks,
                player => player.Id,
                rank => rank.Player.Id,
                (player, rank) => new { player, rank })
            .Select(x => new GetRankedPlayerReadModel
            {
                Id = x.player.Id.Value,
                Age = x.player.Age,
                Country = x.player.Country.Value,
                FirstName = x.player.FirstName,
                LastName = x.player.LastName,
                Gender = x.player.Gender,
                Picture = x.player.Picture,
                Measurement = new GetRankedPlayerReadModel.MeasurementReadModel
                {
                    Height = x.player.Height,
                    Weight = x.player.Weight
                },
                Ranking = new GetRankedPlayerReadModel.RankingReadModel
                {
                    Position = x.rank.Position,
                    Points = x.rank.Points
                }
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
    
    public async Task<ListRankedPlayerReadModel> ListRankedPlayersAsync(CancellationToken cancellationToken)
    {
        var players = await context.Players
            .Join(
                context.Ranks,
                player => player.Id,
                rank => rank.Player.Id,
                (player, rank) => new { player, rank })
            .OrderBy(x => x.rank.Position)
            .Select(x => new ListRankedPlayerReadModel.PlayerReadModel
            {
                Id = x.player.Id.Value,
                FirstName = x.player.FirstName,
                LastName = x.player.LastName,
                Ranking = new ListRankedPlayerReadModel.RankingReadModel
                {
                    Position = x.rank.Position,
                    Points = x.rank.Points
                }
            })
            .ToListAsync(cancellationToken);

        return new ListRankedPlayerReadModel(players);
    }
    
    // A tester avec un TestServer et un DbContext InMemory OU Tests d'intégrations
    public async Task<RetrieveStatisticsReadModel> RetrieveStatisticsAsync(CancellationToken cancellationToken)
    {
        var ranksData = await context.Ranks
            .Select(r => new
            {
                Country = r.Player.Country.Value, 
                r.LastScores
            })
            .ToListAsync(cancellationToken);
        
        var countryWithHighestRatio = ranksData
            .Where(r => r.LastScores.Count != 0)
            .GroupBy(r => r.Country)
            .Select(g => new
            {
                Country = g.Key,
                WinRatio = g.Average(r => r.LastScores.Count(score => score == 1) / r.LastScores.Count)
            })
            .OrderByDescending(x => x.WinRatio)
            .First().Country;

        var averageBmi = await context.Players
            .AverageAsync(p => (p.Weight / 1000.0) / Math.Pow(p.Height / 100.0, 2), cancellationToken);
    
        var playerHeights = await context.Players
            .OrderBy(p => p.Height)
            .Select(p => p.Height)
            .ToListAsync(cancellationToken);

        return new RetrieveStatisticsReadModel
        {
            CountryHighestRation = countryWithHighestRatio,
            AverageBmi = Math.Round(averageBmi),
            MedianPlayerHeight = ComputeMedian(playerHeights)
        };
    }

    private static int ComputeMedian(List<int> values)
    {
        var length = values.Count;
        
        if (length == 0)
            return 0;

        // impair
        if (length % 2 != 0)
            return values[length / 2];
        
        return (values[length / 2 - 1] + values[length / 2]) / 2;
    }
}