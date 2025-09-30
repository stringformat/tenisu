using Tenisu.Application.Common.DAOs;
using Tenisu.Common;
using Tenisu.Domain.RankingAggregate;

namespace Tenisu.Application.Ranking.GetRankedPlayer;

public class GetRankedPlayerUseCase(IRankingDao rankingDao)
{
    public async Task<Result<GetRankedPlayerReadModel>> HandleAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var readModel = await rankingDao.GetRankedPlayerAsync(playerId, cancellationToken);

        if (readModel is null)
            return RankingErrors.RankedPlayerNotFound;
        
        return readModel;
    }
}