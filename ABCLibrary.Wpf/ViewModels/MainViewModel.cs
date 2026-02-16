using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ABCLibrary.Application.DTOs;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Domain.Entities;
using ABCLibrary.Wpf.Commands;

namespace ABCLibrary.Wpf.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly IBookService _bookService;
    private readonly IStudentService _studentService;
    private readonly ICirculationService _circulationService;
    private readonly IReportService _reportService;
    private readonly IBackupService _backupService;

    private DashboardStats _stats = new(0, 0, 0, 0);
    private string _issueBarcode = string.Empty;
    private string _issueRoll = string.Empty;
    private string _returnBarcode = string.Empty;
    private string _message = "Welcome to ABC College Library";

    public MainViewModel(IBookService bookService, IStudentService studentService, ICirculationService circulationService, IReportService reportService, IBackupService backupService)
    {
        _bookService = bookService;
        _studentService = studentService;
        _circulationService = circulationService;
        _reportService = reportService;
        _backupService = backupService;

        Books = new ObservableCollection<Book>();
        Students = new ObservableCollection<Student>();

        RefreshDashboardCommand = new AsyncRelayCommand(LoadDashboardAsync);
        AddSampleBookCommand = new AsyncRelayCommand(AddSampleBookAsync);
        AddSampleStudentCommand = new AsyncRelayCommand(AddSampleStudentAsync);
        IssueBookCommand = new AsyncRelayCommand(IssueBookAsync);
        ReturnBookCommand = new AsyncRelayCommand(ReturnBookAsync);
        ExportInventoryCommand = new AsyncRelayCommand(ExportInventoryAsync);
        BackupCommand = new AsyncRelayCommand(BackupAsync);
        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
    }

    public ObservableCollection<Book> Books { get; }
    public ObservableCollection<Student> Students { get; }

    public int TotalBooks => Stats.TotalBooks;
    public int IssuedBooks => Stats.IssuedBooks;
    public int OverdueBooks => Stats.OverdueBooks;
    public int TotalStudents => Stats.TotalStudents;

    public DashboardStats Stats
    {
        get => _stats;
        private set
        {
            _stats = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalBooks));
            OnPropertyChanged(nameof(IssuedBooks));
            OnPropertyChanged(nameof(OverdueBooks));
            OnPropertyChanged(nameof(TotalStudents));
        }
    }

    public string IssueBarcode { get => _issueBarcode; set => SetProperty(ref _issueBarcode, value); }
    public string IssueRoll { get => _issueRoll; set => SetProperty(ref _issueRoll, value); }
    public string ReturnBarcode { get => _returnBarcode; set => SetProperty(ref _returnBarcode, value); }
    public string Message { get => _message; set => SetProperty(ref _message, value); }

    public ICommand RefreshDashboardCommand { get; }
    public ICommand AddSampleBookCommand { get; }
    public ICommand AddSampleStudentCommand { get; }
    public ICommand IssueBookCommand { get; }
    public ICommand ReturnBookCommand { get; }
    public ICommand ExportInventoryCommand { get; }
    public ICommand BackupCommand { get; }
    public ICommand LoadDataCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadDashboardAsync();
        await LoadDataAsync();
    }

    private async Task LoadDashboardAsync() => Stats = await _reportService.GetDashboardStatsAsync();

    private async Task LoadDataAsync()
    {
        Books.Clear();
        Students.Clear();

        foreach (var b in await _bookService.SearchAsync(new BookSearchFilter { Page = 1, PageSize = 100 }))
        {
            Books.Add(b);
        }

        foreach (var s in await _studentService.SearchAsync(null))
        {
            Students.Add(s);
        }
    }

    private async Task AddSampleBookAsync()
    {
        await _bookService.CreateBookAsync(new CreateBookRequest
        {
            Isbn = $"978-{DateTime.Now:HHmmss}",
            Title = "Sample Distributed Systems",
            Author = "ABC Faculty",
            Publisher = "ABC Press",
            Category = "Technology",
            PublishedYear = 2025,
            ShelfLocation = "A-12",
            CopyCount = 2
        });

        Message = "Sample book created.";
        await InitializeAsync();
    }

    private async Task AddSampleStudentAsync()
    {
        await _studentService.AddStudentAsync(new Student
        {
            RollNumber = $"ABC-{DateTime.Now:HHmmss}",
            FullName = "Sample Student",
            Department = "CSE",
            Course = "B.Tech",
            Year = 2,
            ContactNumber = "9999999999",
            Email = "student@abc.edu"
        });

        Message = "Sample student added.";
        await InitializeAsync();
    }

    private async Task IssueBookAsync()
    {
        await _circulationService.IssueAsync(new IssueBookRequest(IssueBarcode, IssueRoll, 14, "admin"));
        Message = "Book issued successfully.";
        await InitializeAsync();
    }

    private async Task ReturnBookAsync()
    {
        var result = await _circulationService.ReturnAsync(new ReturnBookRequest(ReturnBarcode, 5, "admin"));
        Message = $"Book returned. Fine: {result.FineAmount:C}";
        await InitializeAsync();
    }

    private async Task ExportInventoryAsync()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "InventoryReport.xlsx");
        var output = await _reportService.ExportInventoryReportAsync(path);
        Message = $"Report exported: {output}";
    }

    private async Task BackupAsync()
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ABCLibraryBackups");
        var output = await _backupService.CreateBackupAsync(folder, "admin");
        Message = $"Backup created: {output}";
        MessageBox.Show(Message, "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
