using ABCLibrary.Application.Interfaces;
using ABCLibrary.Infrastructure.Data;
using ABCLibrary.Infrastructure.Repositories;
using ABCLibrary.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ABCLibrary.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dbPath)
    {
        var dbFolder = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrWhiteSpace(dbFolder))
        {
            Directory.CreateDirectory(dbFolder);
        }

        services.AddDbContext<LibraryDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBarcodeService, BarcodeService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ICirculationService, CirculationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IBackupService, BackupService>();
        services.AddScoped<ILabelPrinterService, TscLabelPrinterService>();

        return services;
    }
}
