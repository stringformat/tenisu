using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Tenisu.Domain.RankingAggregate.PlayerEntity;
using Tenisu.Domain.RankingAggregate.RankEntity;

namespace Tenisu.Infrastructure.Common;

public class TenisuReadContext(DbContextOptions<TenisuReadContext> options) : DbContext(options)
{
    public DbSet<Domain.RankingAggregate.Ranking> Rankings { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Player> Players { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

public class TenisuReadContextFactory : IDesignTimeDbContextFactory<TenisuReadContext>
{
    public TenisuReadContext CreateDbContext(string[] args)
    {
        throw new InvalidOperationException("Design-time context creation is not supported for read context.");
    }
}