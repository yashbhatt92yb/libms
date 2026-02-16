using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Entities;
using ABCLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ABCLibrary.Infrastructure.Services;

public sealed class StudentService : IStudentService
{
    private readonly LibraryDbContext _dbContext;
    private readonly ILoggingService _loggingService;

    public StudentService(LibraryDbContext dbContext, ILoggingService loggingService)
    {
        _dbContext = dbContext;
        _loggingService = loggingService;
    }

    public async Task<Student> AddStudentAsync(Student student, CancellationToken cancellationToken = default)
    {
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _loggingService.LogAsync("STUDENT_CREATE", $"Student added: {student.RollNumber}");
        return student;
    }

    public async Task<IReadOnlyList<Student>> SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        var studentQuery = _dbContext.Students.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query))
        {
            studentQuery = studentQuery.Where(x => x.RollNumber.Contains(query) || x.FullName.Contains(query));
        }

        return await studentQuery.OrderBy(x => x.RollNumber).ToListAsync(cancellationToken);
    }
}
