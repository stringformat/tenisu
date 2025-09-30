using Tenisu.Application.Common.Repositories;
using Tenisu.Domain.RankingAggregate;
using Tenisu.Infrastructure.Common;

namespace Tenisu.Infrastructure.Ranking;

public class RankingRepository(TenisuContext context) : RepositoryBase<Domain.RankingAggregate.Ranking, RankingId>(context), IRankingRepository;