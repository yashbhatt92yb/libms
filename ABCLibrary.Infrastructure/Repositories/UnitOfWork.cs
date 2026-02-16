using ABCLibrary.Application.Interfaces;
using ABCLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ABCLibrary.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _dbContext;

    public UnitOfWork(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        await operation();
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
