using System.Collections.Immutable;
using Tenisu.Common;
using Tenisu.Domain.RankingAggregate.PlayerEntity;
using Tenisu.Domain.RankingAggregate.RankEntity;

namespace Tenisu.Domain.RankingAggregate;

public class Ranking : Entity<RankingId>, IAggregateRoot
{
    public IReadOnlyCollection<Rank> Ranks => _ranks.ToImmutableList();
    private List<Rank> _ranks;
    
    // ORM
    public Ranking()
    {
        Id = new RankingId(Guid.CreateVersion7());
        _ranks = [];
    }

    public Result RankPlayer(Player player, int position, int points, int[] lastScores)
    {
        if (lastScores.Length == 0 || lastScores.Any(x => x > 1))
            return RankingErrors.IncorrectScore;
        
        if (_ranks.Any(x => x.Player == player))
            return RankingErrors.AlreadyRanked;

        if (_ranks.Any(x => x.Position == position))
        {
            var currentPosition = position;
            var positionsToUpdate = new List<Rank>();
            
            while (_ranks.Any(x => x.Position == currentPosition))
            {
                var rank = _ranks.First(x => x.Position == currentPosition);
                positionsToUpdate.Add(rank);
                currentPosition++;
            }
            
            for (var i = positionsToUpdate.Count - 1; i >= 0; i--)
            {
                positionsToUpdate[i].UpdatePosition(positionsToUpdate[i].Position + 1);
            }
        }

        _ranks.Add(Rank.Create(player, position, points, lastScores));
        _ranks = _ranks.OrderBy(x => x.Position).ToList();
        
        return Result.Success;
    }
}