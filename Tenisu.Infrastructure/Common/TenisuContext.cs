using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tenisu.Infrastructure.Common;

public class TenisuContext(DbContextOptions<TenisuContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

public class TenisuContextContextFactory : IDesignTimeDbContextFactory<TenisuContext>
{
    public TenisuContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenisuContext>();
        optionsBuilder.UseSqlServer("Server=tcp:sql-dev-fce.database.windows.net,1433;Initial Catalog=tenisu;Persist Security Info=False;User ID=alexis;Password=@lexisdev01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        
        return new TenisuContext(optionsBuilder.Options);
    }
}