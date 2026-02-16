using ABCLibrary.Application.DTOs;
using ABCLibrary.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ABCLibrary.Infrastructure.Data;

namespace ABCLibrary.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly LibraryDbContext _dbContext;
    private readonly ILoggingService _loggingService;

    public AuthService(LibraryDbContext dbContext, ILoggingService loggingService)
    {
        _dbContext = dbContext;
        _loggingService = loggingService;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.IsActive, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            await _loggingService.LogAsync("LOGIN_FAILED", $"Failed login for {request.Username}", request.Username, cancellationToken: cancellationToken);
            return new LoginResult(false, "Invalid username or password.");
        }

        user.LastLoginAtUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _loggingService.LogAsync("LOGIN_SUCCESS", "User logged in.", user.Username, cancellationToken: cancellationToken);
        return new LoginResult(true, "Login successful.", user.Role);
    }
}
