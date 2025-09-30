using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Tenisu.Application.Common.DAOs;
using Tenisu.Application.Ranking.GetRankedPlayer;
using Tenisu.Domain.RankingAggregate;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Tests.Application.UseCase;

public class GetRankedPlayerUseCaseTests
{
    [Fact]
    public async Task HandleAsync_WithValidPlayerId_ShouldGetRankedPlayer()
    {
        // Arrange
        var dao = Substitute.For<IRankingDao>();
        var getRankedPlayerReadModel = new GetRankedPlayerReadModel
        {
            Id = Guid.NewGuid(),
            FirstName = "Jean",
            LastName = "Charles",
            Gender = Gender.M,
            Age = 30,
            Picture = new Uri("https://test.com/test.jpg"),
            Country = "",
            Measurement = new GetRankedPlayerReadModel.MeasurementReadModel
            {
                Height = 150,
                Weight = 50000,
            },
            Ranking = new GetRankedPlayerReadModel.RankingReadModel
            {
                Points = 1000,
                Position = 1
            }
        };
        
        dao.GetRankedPlayerAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(getRankedPlayerReadModel);
        
        var sut = new GetRankedPlayerUseCase(dao);
        var playerId = Guid.NewGuid();
        
        // Act
        var result = await sut.HandleAsync(playerId, CancellationToken.None);

        // Assert
        await dao.Received(1).GetRankedPlayerAsync(Arg.Is<Guid>(x => x == playerId), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_WithUnknowPlayerId_ShouldReturnRankedPlayerNotFoundError()
    {
        // Arrange
        var dao = Substitute.For<IRankingDao>();
        dao.GetRankedPlayerAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNullForAnyArgs();
        
        var sut = new GetRankedPlayerUseCase(dao);
        var playerId = Guid.NewGuid();
        
        // Act
        var result = await sut.HandleAsync(playerId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(RankingErrors.RankedPlayerNotFound);
    }
}