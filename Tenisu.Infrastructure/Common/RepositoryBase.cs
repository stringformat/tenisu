using Microsoft.EntityFrameworkCore;
using Tenisu.Common;

namespace Tenisu.Infrastructure.Common;

public abstract class RepositoryBase<TEntity, TId>(TenisuContext dbContext) 
    where TEntity : Entity<TId>, IAggregateRoot
    where TId : IEquatable<TId>
{
    public async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<bool> Exist(TId id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().AnyAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }
    
    public async Task RemoveByIdAsync(TId id, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
    }
}