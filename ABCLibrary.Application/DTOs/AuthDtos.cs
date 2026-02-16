namespace ABCLibrary.Application.DTOs;

public sealed record LoginRequest(string Username, string Password);
public sealed record LoginResult(bool IsSuccess, string Message, string? Role = null);
