using System.Windows;
using System.Windows.Input;
using ABCLibrary.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;
using ABCLibrary.Application.Interfaces;
using ABCLibrary.Wpf.Commands;

namespace ABCLibrary.Wpf.ViewModels;

public sealed class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private string _username = "admin";
    private string _password = "Admin@123";
    private string _statusMessage = "";

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new AsyncRelayCommand(LoginAsync, () => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ICommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        var result = await _authService.LoginAsync(new LoginRequest(Username, Password));
        StatusMessage = result.Message;
        if (!result.IsSuccess)
        {
            return;
        }

        var mainWindow = App.Services.GetRequiredService<Views.MainWindow>();
        mainWindow.Show();
        Application.Current.Windows.OfType<Views.LoginWindow>().FirstOrDefault()?.Close();
    }
}
