using ABCLibrary.Application.DTOs;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Entities;
using ABCLibrary.Domain.Enums;
using ABCLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Services;

public sealed class CirculationService : ICirculationService
{
    private const int MaxBorrowLimit = 3;

    private readonly LibraryDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggingService _loggingService;

    public CirculationService(LibraryDbContext dbContext, IUnitOfWork unitOfWork, ILoggingService loggingService)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _loggingService = loggingService;
    }

    public async Task IssueAsync(IssueBookRequest request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.RollNumber == request.RollNumber && x.IsActive, cancellationToken)
                ?? throw new InvalidOperationException("Active student not found.");

            var activeBorrowCount = await _dbContext.Transactions.CountAsync(x => x.StudentId == student.Id && x.Status == TransactionStatus.Issued, cancellationToken);
            if (activeBorrowCount >= MaxBorrowLimit)
            {
                throw new InvalidOperationException("Borrow limit exceeded.");
            }

            var copy = await _dbContext.BookCopies.FirstOrDefaultAsync(x => x.BarcodeNumber == request.BarcodeNumber && x.IsAvailable, cancellationToken)
                ?? throw new InvalidOperationException("Book copy unavailable.");

            copy.IsAvailable = false;
            _dbContext.Transactions.Add(new LibraryTransaction
            {
                StudentId = student.Id,
                BookCopyId = copy.Id,
                IssuedAtUtc = DateTime.UtcNow,
                DueAtUtc = DateTime.UtcNow.AddDays(request.LoanDays),
                Status = TransactionStatus.Issued
            });
        }, cancellationToken);

        await _loggingService.LogAsync("BOOK_ISSUE", $"Issued {request.BarcodeNumber} to {request.RollNumber}", request.IssuedBy, cancellationToken: cancellationToken);
    }

    public async Task<ReturnBookResult> ReturnAsync(ReturnBookRequest request, CancellationToken cancellationToken = default)
    {
        decimal fine = 0;
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var tx = await _dbContext.Transactions
                .Include(x => x.BookCopy)
                .FirstOrDefaultAsync(x => x.BookCopy!.BarcodeNumber == request.BarcodeNumber && x.Status == TransactionStatus.Issued, cancellationToken)
                ?? throw new InvalidOperationException("Active issue not found.");

            tx.ReturnedAtUtc = DateTime.UtcNow;
            tx.Status = TransactionStatus.Returned;
            tx.BookCopy!.IsAvailable = true;

            var overdueDays = Math.Max(0, (DateTime.UtcNow.Date - tx.DueAtUtc.Date).Days);
            fine = overdueDays * request.DailyFineRate;
            tx.FineAmount = fine;
        }, cancellationToken);

        await _loggingService.LogAsync("BOOK_RETURN", $"Returned {request.BarcodeNumber}, fine: {fine}", request.ReturnedBy, cancellationToken: cancellationToken);
        return new ReturnBookResult(true, fine, "Book returned successfully.");
    }
}
