namespace ABCLibrary.Domain.Entities;

public sealed class Student : BaseEntity
{
    public required string RollNumber { get; set; }
    public required string FullName { get; set; }
    public required string Department { get; set; }
    public required string Course { get; set; }
    public short Year { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<LibraryTransaction> Transactions { get; set; } = new List<LibraryTransaction>();
}
