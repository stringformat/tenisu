using System.Net;
using Tenisu.Common;
using Tenisu.Domain.Common;

namespace Tenisu.Domain.RankingAggregate;

public static class RankingErrors
{
    public static Error AlreadyRanked
        => new(
            Message: "Player already ranked.", 
            Code: Errors.Ranking_AlreadyRanked,
            StatusCode: HttpStatusCode.BadRequest);
    
    public static Error RankedPlayerNotFound
        => new(
            Message: "Ranking player not found.", 
            Code: Errors.Ranking_RankedPlayerNotFound,
            StatusCode: HttpStatusCode.NotFound);
    
    public static Error IncorrectScore
        => new(
            Message: "Score incorrect.", 
            Code: Errors.Ranking_IncorrectScore,
            StatusCode: HttpStatusCode.BadRequest);
}