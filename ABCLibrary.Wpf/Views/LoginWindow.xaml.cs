using System.Windows;

namespace ABCLibrary.Wpf.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(ViewModels.LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
