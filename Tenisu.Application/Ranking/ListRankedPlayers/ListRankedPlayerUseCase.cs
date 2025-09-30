using Tenisu.Application.Common.DAOs;

namespace Tenisu.Application.Ranking.ListRankedPlayers;

public class ListRankedPlayerUseCase(IRankingDao rankingDao)
{
    public async Task<ListRankedPlayerReadModel> HandleAsync(CancellationToken cancellationToken)
    {
       return await rankingDao.ListRankedPlayersAsync(cancellationToken);
    }
}