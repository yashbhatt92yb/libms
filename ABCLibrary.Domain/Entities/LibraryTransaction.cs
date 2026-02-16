using ABCLibrary.Domain.Enums;

namespace ABCLibrary.Domain.Entities;

public sealed class LibraryTransaction : BaseEntity
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int BookCopyId { get; set; }
    public BookCopy? BookCopy { get; set; }

    public DateTime IssuedAtUtc { get; set; }
    public DateTime DueAtUtc { get; set; }
    public DateTime? ReturnedAtUtc { get; set; }

    public decimal FineAmount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Issued;
}
