using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenisu.Domain.RankingAggregate.PlayerEntity;
using Tenisu.Domain.RankingAggregate.RankEntity;

namespace Tenisu.Infrastructure.Ranking.Configuration;

public class RankEntityConfiguration : IEntityTypeConfiguration<Rank>
{
    public void Configure(EntityTypeBuilder<Rank> builder)
    {
        builder.ToTable(nameof(Rank));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => new RankId(id));
        
        builder.Property(x => x.Position).IsRequired();
        builder.Property(x => x.Points).IsRequired();
        builder.Property(x => x.LastScores)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(
                scores => JsonSerializer.Serialize(scores, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<int[]>(json, JsonSerializerOptions.Default) ?? Array.Empty<int>())
            .IsRequired();

        builder
            .HasOne(x => x.Player)
            .WithOne()
            .HasForeignKey<Player>($"{nameof(Rank)}{nameof(Rank.Id)}")
            .IsRequired();
        
        builder.Navigation(x => x.Player).AutoInclude();
    }
}