namespace ABCLibrary.Application.DTOs;

public sealed class CreateBookRequest
{
    public required string Isbn { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public string? Publisher { get; init; }
    public string? Category { get; init; }
    public short PublishedYear { get; init; }
    public string? ShelfLocation { get; init; }
    public int CopyCount { get; init; } = 1;
}

public sealed class BookSearchFilter
{
    public string? Query { get; init; }
    public string? Category { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
}
