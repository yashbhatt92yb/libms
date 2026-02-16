namespace ABCLibrary.Domain.Entities;

public sealed class BookCopy : BaseEntity
{
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public required string BarcodeNumber { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime? LastPrintedAtUtc { get; set; }

    public ICollection<LibraryTransaction> Transactions { get; set; } = new List<LibraryTransaction>();
}
