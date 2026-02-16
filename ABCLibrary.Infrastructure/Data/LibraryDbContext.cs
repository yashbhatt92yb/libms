using ABCLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Data;

public sealed class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookCopy> BookCopies => Set<BookCopy>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<LibraryTransaction> Transactions => Set<LibraryTransaction>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<BarcodeReprintLog> BarcodeReprintLogs => Set<BarcodeReprintLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.Username).HasMaxLength(100);
            builder.Property(x => x.PasswordHash).HasMaxLength(300);
        });

        modelBuilder.Entity<Book>(builder =>
        {
            builder.HasIndex(x => x.Isbn);
            builder.Property(x => x.Title).HasMaxLength(250);
            builder.Property(x => x.Author).HasMaxLength(200);
            builder.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<BookCopy>(builder =>
        {
            builder.HasIndex(x => x.BarcodeNumber).IsUnique();
            builder.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<Student>(builder =>
        {
            builder.HasIndex(x => x.RollNumber).IsUnique();
            builder.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<LibraryTransaction>(builder =>
        {
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BookCopy)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.BookCopyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<SystemSetting>(builder =>
        {
            builder.HasIndex(x => x.Key).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
