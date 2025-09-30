using Tenisu.Application.Common.DAOs;

namespace Tenisu.Application.Ranking.RetrieveStatistics;

public class RetrieveStatisticsUseCase(IRankingDao rankingDao)
{
    public async Task<RetrieveStatisticsReadModel> HandleAsync(CancellationToken cancellationToken)
    {
        return await rankingDao.RetrieveStatisticsAsync(cancellationToken);
    }
}