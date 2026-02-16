namespace ABCLibrary.Application.Interfaces;

public interface IUnitOfWork
{
    Task ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
}
