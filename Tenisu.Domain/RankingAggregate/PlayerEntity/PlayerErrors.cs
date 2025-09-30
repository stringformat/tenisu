using System.Net;
using Tenisu.Common;
using Tenisu.Domain.Common;

namespace Tenisu.Domain.RankingAggregate.PlayerEntity;

public static class PlayerErrors
{
    public static Error CountryCodeIncorrect
        => new(
            Message: "Le code est incorrect", 
            Code: Errors.Country_CodeIncorrect,
            StatusCode: HttpStatusCode.BadRequest);
}