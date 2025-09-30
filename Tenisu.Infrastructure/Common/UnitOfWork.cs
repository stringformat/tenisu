using Tenisu.Common;

namespace Tenisu.Infrastructure.Common;

public class UnitOfWork(TenisuContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}