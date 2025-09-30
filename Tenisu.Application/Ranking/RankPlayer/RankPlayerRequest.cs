using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Application.Ranking.RankPlayer;

public record RankPlayerRequest(
    RankPlayerRequest.PlayerRequest Player,
    RankPlayerRequest.RankRequest Rank,
    RankPlayerRequest.MeasurementRequest Measurement,
    string Country)
{
    public record PlayerRequest(
        string FirstName,
        string LastName,
        Gender Gender,
        int Age,
        Uri Picture);

    public record RankRequest(
        int Position, 
        int Points,
        int[] LastScores);
    
    public record MeasurementRequest(
        int Height,
        int Weight);
}