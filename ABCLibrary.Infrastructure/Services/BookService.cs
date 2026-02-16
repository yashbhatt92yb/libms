using ABCLibrary.Application.DTOs;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Entities;
using ABCLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Services;

public sealed class BookService : IBookService
{
    private readonly LibraryDbContext _dbContext;
    private readonly IBarcodeService _barcodeService;
    private readonly ILoggingService _loggingService;

    public BookService(LibraryDbContext dbContext, IBarcodeService barcodeService, ILoggingService loggingService)
    {
        _dbContext = dbContext;
        _barcodeService = barcodeService;
        _loggingService = loggingService;
    }

    public async Task<Book> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Isbn = request.Isbn,
            Title = request.Title,
            Author = request.Author,
            Publisher = request.Publisher,
            Category = request.Category,
            PublishedYear = request.PublishedYear,
            ShelfLocation = request.ShelfLocation
        };

        var existingMaxBarcode = await _dbContext.BookCopies
            .OrderByDescending(x => x.Id)
            .Select(x => x.BarcodeNumber)
            .FirstOrDefaultAsync(cancellationToken);

        long sequence = 0;
        if (!string.IsNullOrWhiteSpace(existingMaxBarcode) && long.TryParse(existingMaxBarcode.Replace("LIB-", string.Empty), out var parsed))
        {
            sequence = parsed;
        }

        for (var i = 0; i < request.CopyCount; i++)
        {
            sequence++;
            book.Copies.Add(new BookCopy
            {
                BarcodeNumber = _barcodeService.BuildNextBarcode(sequence),
                IsAvailable = true
            });
        }

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _loggingService.LogAsync("BOOK_CREATE", $"Book added: {book.Title} ({request.CopyCount} copies)");
        return book;
    }

    public async Task<IReadOnlyList<Book>> SearchAsync(BookSearchFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Books.Include(x => x.Copies).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            query = query.Where(x => x.Title.Contains(filter.Query) || x.Author.Contains(filter.Query) || x.Isbn.Contains(filter.Query));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            query = query.Where(x => x.Category == filter.Category);
        }

        return await query
            .OrderBy(x => x.Title)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task ReprintBarcodeAsync(string barcode, string reason, string username, CancellationToken cancellationToken = default)
    {
        var copy = await _dbContext.BookCopies.FirstOrDefaultAsync(x => x.BarcodeNumber == barcode, cancellationToken)
            ?? throw new InvalidOperationException("Barcode not found.");

        copy.LastPrintedAtUtc = DateTime.UtcNow;
        _dbContext.BarcodeReprintLogs.Add(new BarcodeReprintLog
        {
            BookCopyId = copy.Id,
            Reason = reason,
            ReprintedBy = username,
            CreatedAtUtc = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _loggingService.LogAsync("BARCODE_REPRINT", $"Reprinted {barcode}. Reason: {reason}", username, cancellationToken: cancellationToken);
    }
}
