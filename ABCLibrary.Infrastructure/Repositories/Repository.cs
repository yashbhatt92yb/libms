using System.Linq.Expressions;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly LibraryDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(LibraryDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await DbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await DbContext.SaveChangesAsync(cancellationToken);
}
