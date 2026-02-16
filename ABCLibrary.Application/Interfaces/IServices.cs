using ABCLibrary.Application.DTOs;
using ABCLibrary.Domain.Entities;

namespace ABCLibrary.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

public interface IBookService
{
    Task<Book> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> SearchAsync(BookSearchFilter filter, CancellationToken cancellationToken = default);
    Task ReprintBarcodeAsync(string barcode, string reason, string username, CancellationToken cancellationToken = default);
}

public interface IStudentService
{
    Task<Student> AddStudentAsync(Student student, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Student>> SearchAsync(string? query, CancellationToken cancellationToken = default);
}

public interface ICirculationService
{
    Task IssueAsync(IssueBookRequest request, CancellationToken cancellationToken = default);
    Task<ReturnBookResult> ReturnAsync(ReturnBookRequest request, CancellationToken cancellationToken = default);
}

public interface IBarcodeService
{
    byte[] GenerateCode128Png(string value, int width = 300, int height = 80);
    string BuildNextBarcode(long sequence);
}

public interface IReportService
{
    Task<DashboardStats> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
    Task<string> ExportInventoryReportAsync(string outputPath, CancellationToken cancellationToken = default);
}

public interface IBackupService
{
    Task<string> CreateBackupAsync(string backupFolder, string username, CancellationToken cancellationToken = default);
}

public interface ILoggingService
{
    Task LogAsync(string action, string description, string? username = null, string? metadataJson = null, CancellationToken cancellationToken = default);
}
