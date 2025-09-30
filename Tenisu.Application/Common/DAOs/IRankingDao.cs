using Tenisu.Application.Ranking.GetRankedPlayer;
using Tenisu.Application.Ranking.ListRankedPlayers;
using Tenisu.Application.Ranking.RetrieveStatistics;

namespace Tenisu.Application.Common.DAOs;

public interface IRankingDao
{
    Task<GetRankedPlayerReadModel?> GetRankedPlayerAsync(Guid id, CancellationToken cancellationToken);
    Task<ListRankedPlayerReadModel> ListRankedPlayersAsync(CancellationToken cancellationToken);
    Task<RetrieveStatisticsReadModel> RetrieveStatisticsAsync(CancellationToken cancellationToken);
}