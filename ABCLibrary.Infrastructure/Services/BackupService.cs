using ABCLibrary.Application.Interfaces;
using ABCLibrary.Infrastructure.Data;

namespace ABCLibrary.Infrastructure.Services;

public sealed class BackupService : IBackupService
{
    private readonly ILoggingService _loggingService;

    public BackupService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task<string> CreateBackupAsync(string backupFolder, string username, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(backupFolder);

        var sourceFile = DbBootstrapper.DefaultDbPath;
        var backupFile = Path.Combine(backupFolder, $"library-backup-{DateTime.Now:yyyyMMdd-HHmmss}.db");
        File.Copy(sourceFile, backupFile, overwrite: true);

        await _loggingService.LogAsync("BACKUP", $"Backup created at {backupFile}", username, cancellationToken: cancellationToken);
        return backupFile;
    }
}
