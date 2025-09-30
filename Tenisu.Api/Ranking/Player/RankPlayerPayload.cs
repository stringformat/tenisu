using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tenisu.Application.Ranking.RankPlayer;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Api.Ranking.Player;

public record RankPlayerPayload(
    [property: MinLength(1)]string FirstName,
    [property: MinLength(1)]string LastName,
    [property: Range(1, 99)]int Age,
    Gender Gender,
    Uri Picture,
    RankPlayerPayload.CountryPayload Country,
    RankPlayerPayload.MeasurementPayload Measurement,
    RankPlayerPayload.RankPayload Rank)
{
    [JsonIgnore]
    public RankPlayerRequest ToUseCaseRequest => new (
        Player: new RankPlayerRequest.PlayerRequest(FirstName, LastName, Gender, Age, Picture),
        Rank: new RankPlayerRequest.RankRequest(Rank.Position, Rank.Points, Rank.LastScores),
        Measurement: new RankPlayerRequest.MeasurementRequest(Measurement.Height, Measurement.Weight),
        Country: Country.Code);
    
    public record CountryPayload([property: MinLength(3)][property: MaxLength(3)]string Code);

    public record RankPayload(
        [property: Range(1, 9999)] int Position,
        [property: Range(1, 9999)] int Points,
        [property: MinLength(1)] int[] LastScores);

    public record MeasurementPayload(
        [property: Range(50,250)]int Height,
        [property: Range(10000,250000)]int Weight);
}