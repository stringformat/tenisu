using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Application.Ranking.GetRankedPlayer;

public record GetRankedPlayerReadModel
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string ShortName => $"{FirstName[..1].ToUpper()}.{LastName[..3].ToUpper()}";
    public required Gender Gender { get; init; }
    public required int Age { get; init; }
    public required Uri Picture { get; init; }
    public required string Country { get; init; }
    public required MeasurementReadModel Measurement { get; init; }
    public required RankingReadModel Ranking { get; init; }

    public record MeasurementReadModel
    {
        public required int Height { get; init; }
        public required int Weight { get; init; }
    }

    public record RankingReadModel
    {
        public required int Position { get; init; }
        public required int Points { get; init; }
    }
}