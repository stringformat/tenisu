using System.Collections.Immutable;
using Tenisu.Common;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Domain.RankingAggregate.RankEntity;

public class Rank : Entity<RankId>
{
    public Player Player { get; }
    
    public int Position { get; private set; }

    public int Points { get; private set; }

    public IReadOnlyCollection<int> LastScores => _lastScores.ToImmutableArray();
    private readonly int[] _lastScores;
    
    // ORM
    private Rank()
    {
    }

    private Rank(Player player, int position, int points, int[] lastScores)
    {
        Id = new RankId(Guid.CreateVersion7());
        Player = player;
        Position = position;
        Points = points;
        _lastScores = lastScores;
    }

    public static Rank Create(Player player, int position, int points, int[] lastScores)
    {
        return new Rank(player, position, points, lastScores);
    }

    public void UpdatePosition(int position)
    {
        Position = position;
    }
}