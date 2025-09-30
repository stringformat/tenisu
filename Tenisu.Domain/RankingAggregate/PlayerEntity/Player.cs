using Tenisu.Common;

namespace Tenisu.Domain.RankingAggregate.PlayerEntity;

public class Player : Entity<PlayerId>
{
    public Country Country { get; }
    public string FirstName { get; }
    public string LastName { get;}
    public Gender Gender { get; }
    public int Height { get; }
    public int Weight { get; }
    public int Age { get; }
    public Uri Picture { get; }

    // ORM
    private Player()
    {
    }

    private Player(
        Country country,
        string firstName,
        string lastName,
        Gender gender, 
        int  height,
        int weight,
        int age,
        Uri picture)
    {
        Id = new PlayerId(Guid.CreateVersion7());
        Country = country;                           
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Height = height;
        Weight = weight;
        Age = age;
        Picture = picture;
    }

    public static Result<Player> Create(
        string firstname,
        string lastname,
        int age,
        Gender gender, 
        Uri picture, 
        int height, 
        int weight,
        string countryCode)
    {
        var (isSuccess, country, error) = Country.Create(countryCode);
        
        if (!isSuccess)
            return error;

        return new Player(country, firstname, lastname, gender, height, weight, age, picture);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null || GetType() != obj.GetType()) return false;

        var other = (Player)obj;

        return string.Equals(Country.Value, other.Country.Value, StringComparison.InvariantCultureIgnoreCase)
               && string.Equals(FirstName, other.FirstName, StringComparison.InvariantCultureIgnoreCase)
               && string.Equals(LastName, other.LastName, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Country.Value, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add(FirstName, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add(LastName, StringComparer.InvariantCultureIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Player? left, Player? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Player? left, Player? right) => !(left == right);
}