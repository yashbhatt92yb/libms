namespace ABCLibrary.Domain.Entities;

public sealed class User : BaseEntity
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; } = "Librarian";
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAtUtc { get; set; }
}
