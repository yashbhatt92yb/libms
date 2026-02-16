using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Entities;
using ABCLibrary.Infrastructure.Data;

namespace ABCLibrary.Infrastructure.Services;

public sealed class LoggingService : ILoggingService
{
    private readonly LibraryDbContext _dbContext;

    public LoggingService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogAsync(string action, string description, string? username = null, string? metadataJson = null, CancellationToken cancellationToken = default)
    {
        _dbContext.SystemLogs.Add(new SystemLog
        {
            Action = action,
            Description = description,
            Username = username,
            MetadataJson = metadataJson,
            CreatedAtUtc = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
