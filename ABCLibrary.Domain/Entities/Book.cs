namespace ABCLibrary.Domain.Entities;

public sealed class Book : BaseEntity
{
    public required string Isbn { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Publisher { get; set; }
    public string? Category { get; set; }
    public short PublishedYear { get; set; }
    public string? ShelfLocation { get; set; }

    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
}
