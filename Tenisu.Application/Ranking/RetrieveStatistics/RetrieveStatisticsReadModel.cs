namespace Tenisu.Application.Ranking.RetrieveStatistics;

public record RetrieveStatisticsReadModel
{
    public required string CountryHighestRation { get; init; }
    public required double AverageBmi { get; init; }
    public required int MedianPlayerHeight { get; init; }
}