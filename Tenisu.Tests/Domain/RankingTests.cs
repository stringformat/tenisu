using FluentAssertions;
using Tenisu.Domain.RankingAggregate;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Tests.Domain;

public class RankingTests
{
    [Fact]
    public void RankPlayer_WithValidData_ShouldAddPlayerToRanking()
    {
        // Arrange
        var ranking = new Ranking();
        
        var expectedPlayer = CreateTestPlayer();
        var expectedPosition = 1;
        var expectedPoints = 1000;

        // Act
        var result = ranking.RankPlayer(expectedPlayer, expectedPosition, expectedPoints, [1, 0, 1, 1, 0]);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ranking.Ranks.Should().HaveCount(1);
        ranking.Ranks.First().Player.Should().Be(expectedPlayer);
        ranking.Ranks.First().Position.Should().Be(expectedPosition);
        ranking.Ranks.First().Points.Should().Be(expectedPoints);
    }

    [Fact]
    public void RankPlayer_WithIncorrectScore_ShouldReturnIncorrectScoreError()
    {
        // Arrange
        var ranking = new Ranking();
        var player = CreateTestPlayer();
        int[] invalidScores = [1, 0, 2, 1];

        // Act
        var result = ranking.RankPlayer(player, 1, 1000, invalidScores);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Should().Be(RankingErrors.IncorrectScore);
        ranking.Ranks.Should().BeEmpty();
    }
    
    [Fact]
    public void RankPlayer_WithEmptyLastScores_ShouldReturnIncorrectScoreError()
    {
        // Arrange
        var ranking = new Ranking();
        var player = CreateTestPlayer();
        int[] emptyScores = [];

        // Act
        var result = ranking.RankPlayer(player, 1, 1000, emptyScores);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Should().Be(RankingErrors.IncorrectScore);
        ranking.Ranks.Should().BeEmpty();
    }

    [Fact]
    public void RankPlayer_WithAlreadyRankedPlayer_ShouldReturnAlreadyRankedError()
    {
        // Arrange
        var ranking = new Ranking();
        var player = CreateTestPlayer();
        int[] scores = [1, 0, 1];
        
        ranking.RankPlayer(player, 1, 1000, scores);

        // Act
        var result = ranking.RankPlayer(player, 2, 800, scores);

        // Assert
        result.IsFail.Should().BeTrue();
        result.Error.Should().Be(RankingErrors.AlreadyRanked);
        ranking.Ranks.Should().HaveCount(1);
    }
    
    /// <summary>
    /// 6 => [2, 18, 25, 35] = [2, 6, 18, 25, 35]
    /// 18 => [2, 18, 25, 35] = [2, 18, 19, 25, 35]
    /// 37 => [2, 18, 25, 35] = [2, 18, 25, 35, 37]
    /// 2 => [2, 18, 25, 35] = [2, 3, 18, 25, 35]
    /// </summary>
    /// <param name="insertPosition"></param>
    /// <param name="expectedPositions"></param>
    [Theory]
    [InlineData(6, new[] { 2, 6, 18, 25, 35 })]
    [InlineData(18, new[] { 2, 18, 19, 25, 35 })]
    [InlineData(37, new[] { 2, 18, 25, 35, 37 })]
    [InlineData(2, new[] { 2, 3, 18, 25, 35 })]
    public void RankPlayer_OnlyShiftsConsecutivePositions(int insertPosition, int[] expectedPositions)
    {
        // Arrange
        var ranking = new Ranking();
        var player1 = CreateTestPlayer("Player1");
        var player2 = CreateTestPlayer("Player2");
        var player3 = CreateTestPlayer("Player3");
        var player4 = CreateTestPlayer("Player4");
        var newPlayer = CreateTestPlayer("NewPlayer");

        ranking.RankPlayer(player1, 2, default, [0]);
        ranking.RankPlayer(player2, 18, default, [0]);
        ranking.RankPlayer(player3, 25, default, [0]);
        ranking.RankPlayer(player4, 35, default, [0]);

        // Act
        var result = ranking.RankPlayer(newPlayer, insertPosition, default, [0]);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var positions = ranking.Ranks.Select(r => r.Position).ToArray();
        positions.Should().Equal(expectedPositions);
    }
    
    private static Player CreateTestPlayer(string? firstName = null, string? lastName = null)
    {
        var result = Player.Create(
            firstname: firstName ?? "Jean",
            lastname: lastName ?? "Maurice",
            age: 25,
            gender: Gender.M,
            picture: new Uri("https://test.com/picture.jpg"),
            height: 180,
            weight: 75,
            countryCode: "FRA"
        );

        return result.Value;
    }
}