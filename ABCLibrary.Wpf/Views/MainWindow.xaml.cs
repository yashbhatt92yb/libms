using System.Windows;

namespace ABCLibrary.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly ViewModels.MainViewModel _viewModel;

    public MainWindow(ViewModels.MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.InitializeAsync();
    }
}
