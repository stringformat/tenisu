using FluentAssertions;
using NSubstitute;
using Tenisu.Application.Common.Repositories;
using Tenisu.Application.Ranking.RankPlayer;
using Tenisu.Common;
using Tenisu.Domain.RankingAggregate;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Tests.Application.UseCase;

public class RankPlayerUseCaseTests
{
    private readonly IRankingRepository _rankingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RankPlayerUseCase _sut;
    private readonly Ranking _ranking;

    public RankPlayerUseCaseTests()
    {
        _rankingRepository = Substitute.For<IRankingRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _ranking = new Ranking();
        _rankingRepository
            .ListAsync(Arg.Any<CancellationToken>())
            .Returns([_ranking]);
        
        _sut = new RankPlayerUseCase(
            _rankingRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldRankPlayerAndSave()
    {
        // Arrange

        // Act
        var result = await _sut.HandleAsync(CreateRequest, CancellationToken.None);

        // Assert
        await _rankingRepository.Received(1)
            .ListAsync(Arg.Any<CancellationToken>());
        
        await _unitOfWork.Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
        
        result.IsSuccess.Should().BeTrue();
        _ranking.Ranks.Should().HaveCount(1);
    }

    [Fact]
    public async Task HandleAsync_WhenInvalidData_ShouldReturnError()
    {
        // Arrange
        var request = CreateRequest with { Country = "AAAA" };
        
        // Act
        var result = await _sut.HandleAsync(request, CancellationToken.None);

        // Assert
        await _rankingRepository.Received(1)
            .ListAsync(Arg.Any<CancellationToken>());
        
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(PlayerErrors.CountryCodeIncorrect);
        _ranking.Ranks.Should().HaveCount(0);
    }

    private static RankPlayerRequest CreateRequest => new(
        Player: new RankPlayerRequest.PlayerRequest(
            FirstName: "Rafael",
            LastName: "Nadal",
            Gender: Gender.M,
            Age: 37,
            Picture: new Uri("https://example.com/players/nadal.jpg")
        ),
        Rank: new RankPlayerRequest.RankRequest(
            Position: 1,
            Points: 9500,
            LastScores: [1, 1, 1, 1, 0]
        ),
        Measurement: new RankPlayerRequest.MeasurementRequest(
            Height: 185,
            Weight: 85
        ),
        Country: "ESP"
    );
}