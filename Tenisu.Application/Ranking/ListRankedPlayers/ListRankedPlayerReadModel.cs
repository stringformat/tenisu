namespace Tenisu.Application.Ranking.ListRankedPlayers;

public record ListRankedPlayerReadModel(IReadOnlyCollection<ListRankedPlayerReadModel.PlayerReadModel> Players)
{
    public record PlayerReadModel
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string ShortName => $"{FirstName[..1].ToUpper()}.{LastName[..3].ToUpper()}";
    
        public required RankingReadModel Ranking { get; init; }
    }

    public record RankingReadModel
    {
        public required int Position { get; init; }
        public required int Points { get; init; }
    }
}