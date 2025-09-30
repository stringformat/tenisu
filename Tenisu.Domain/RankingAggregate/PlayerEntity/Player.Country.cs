using Tenisu.Common;

namespace Tenisu.Domain.RankingAggregate.PlayerEntity;

public record Country
{
    public const int MaxLength = 3;
    public string Value { get; }

    // ORM
    private Country()
    {
    }

    private Country(string value) 
    {
        Value = value;
    }

    public static Result<Country> Create(string code)
    {
        if (code.Length is 0 or > MaxLength)
            return PlayerErrors.CountryCodeIncorrect;
        
        return new Country(code);
    }
}