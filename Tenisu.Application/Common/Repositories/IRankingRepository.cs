namespace Tenisu.Application.Common.Repositories;

public interface IRankingRepository
{
    Task AddAsync(Domain.RankingAggregate.Ranking entity, CancellationToken cancellationToken);
    Task<List<Domain.RankingAggregate.Ranking>> ListAsync(CancellationToken cancellationToken);
}