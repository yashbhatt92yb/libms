using ABCLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Data;

public static class DbBootstrapper
{
    public const string DefaultDbPath = @"C:\ProgramData\ABCLibrary\library.db";

    public static DbContextOptions<LibraryDbContext> CreateSqliteOptions(string dbPath)
    {
        var builder = new DbContextOptionsBuilder<LibraryDbContext>();
        builder.UseSqlite($"Data Source={dbPath}");
        return builder.Options;
    }

    public static async Task InitializeAsync(LibraryDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await dbContext.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;", cancellationToken);

        if (!await dbContext.Users.AnyAsync(cancellationToken))
        {
            dbContext.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsActive = true
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
