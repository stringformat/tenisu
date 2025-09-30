using Tenisu.Application.Common.Repositories;
using Tenisu.Common;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Application.Ranking.RankPlayer;

public class RankPlayerUseCase(IRankingRepository rankingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result> HandleAsync(RankPlayerRequest request, CancellationToken cancellationToken)
    {
        var ranking = await GetRanking(cancellationToken);
        
        var (isSuccessCreatePlayer, player, createPlayerError) = Player.Create(
            request.Player.FirstName, 
            request.Player.LastName, 
            request.Player.Age, 
            request.Player.Gender, 
            request.Player.Picture, 
            request.Measurement.Height, 
            request.Measurement.Weight,
            request.Country);
        
        if(!isSuccessCreatePlayer)
            return createPlayerError;
        
        var (isFailedRankPlayer, rankPlayerError) = ranking.RankPlayer(
            player, 
            request.Rank.Position, 
            request.Rank.Points, 
            request.Rank.LastScores);

        if(isFailedRankPlayer)
            return rankPlayerError;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success;
    }
    
    private async Task<Domain.RankingAggregate.Ranking> GetRanking(CancellationToken cancellationToken)
    {
        var rankings = await rankingRepository.ListAsync(cancellationToken);

        return rankings.Count == 0 ? throw new InvalidOperationException("Ranking not found.") : rankings.Single();
    }
}