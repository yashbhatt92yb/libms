namespace ABCLibrary.Domain.Entities;

public sealed class BarcodeReprintLog : BaseEntity
{
    public int BookCopyId { get; set; }
    public BookCopy? BookCopy { get; set; }
    public required string Reason { get; set; }
    public required string ReprintedBy { get; set; }
}
