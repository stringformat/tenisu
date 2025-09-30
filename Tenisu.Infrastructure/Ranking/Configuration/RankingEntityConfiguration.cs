using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenisu.Domain.RankingAggregate;

namespace Tenisu.Infrastructure.Ranking.Configuration;

public class RankingEntityConfiguration : IEntityTypeConfiguration<Domain.RankingAggregate.Ranking>
{
    public void Configure(EntityTypeBuilder<Domain.RankingAggregate.Ranking> builder)
    {
        builder.ToTable(nameof(Domain.RankingAggregate.Ranking));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => new RankingId(id))
            .HasColumnName(nameof(RankingId.Id));

        builder
            .HasMany(x => x.Ranks)
            .WithOne();
        
        builder.Navigation(x => x.Ranks).AutoInclude();
    }
}