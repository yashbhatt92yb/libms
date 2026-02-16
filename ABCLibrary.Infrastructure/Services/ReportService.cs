using ABCLibrary.Application.DTOs;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Enums;
using ABCLibrary.Infrastructure.Data;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Services;

public sealed class ReportService : IReportService
{
    private readonly LibraryDbContext _dbContext;

    public ReportService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync(CancellationToken cancellationToken = default)
    {
        var totalBooks = await _dbContext.BookCopies.CountAsync(cancellationToken);
        var issuedBooks = await _dbContext.Transactions.CountAsync(x => x.Status == TransactionStatus.Issued, cancellationToken);
        var overdueBooks = await _dbContext.Transactions.CountAsync(x => x.Status == TransactionStatus.Issued && x.DueAtUtc < DateTime.UtcNow, cancellationToken);
        var totalStudents = await _dbContext.Students.CountAsync(cancellationToken);

        return new DashboardStats(totalBooks, issuedBooks, overdueBooks, totalStudents);
    }

    public async Task<string> ExportInventoryReportAsync(string outputPath, CancellationToken cancellationToken = default)
    {
        var books = await _dbContext.Books.Include(x => x.Copies).ToListAsync(cancellationToken);
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Inventory");

        sheet.Cell(1, 1).Value = "ISBN";
        sheet.Cell(1, 2).Value = "Title";
        sheet.Cell(1, 3).Value = "Author";
        sheet.Cell(1, 4).Value = "Category";
        sheet.Cell(1, 5).Value = "Copies";

        for (var i = 0; i < books.Count; i++)
        {
            var row = i + 2;
            sheet.Cell(row, 1).Value = books[i].Isbn;
            sheet.Cell(row, 2).Value = books[i].Title;
            sheet.Cell(row, 3).Value = books[i].Author;
            sheet.Cell(row, 4).Value = books[i].Category;
            sheet.Cell(row, 5).Value = books[i].Copies.Count;
        }

        workbook.SaveAs(outputPath);
        return outputPath;
    }
}
