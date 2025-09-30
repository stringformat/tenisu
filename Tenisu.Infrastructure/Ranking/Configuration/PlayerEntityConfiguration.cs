using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenisu.Domain.RankingAggregate.PlayerEntity;

namespace Tenisu.Infrastructure.Ranking.Configuration;

public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable(nameof(Player));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                id => new PlayerId(id));
        
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.Height).IsRequired();
        builder.Property(x => x.Weight).IsRequired();
        builder.Property(x => x.Picture).IsRequired();

        builder.ComplexProperty(x => x.Country, propertyBuilder =>
        {
            propertyBuilder.Property(x => x.Value)
                .HasColumnName(nameof(Player.Country))
                .HasMaxLength(Country.MaxLength)
                .IsRequired();
        });
    }
}