namespace ABCLibrary.Application.DTOs;

public sealed record IssueBookRequest(string BarcodeNumber, string RollNumber, int LoanDays, string IssuedBy);
public sealed record ReturnBookRequest(string BarcodeNumber, decimal DailyFineRate, string ReturnedBy);
public sealed record ReturnBookResult(bool Success, decimal FineAmount, string Message);
