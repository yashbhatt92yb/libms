using System.Windows;
using ABCLibrary.Infrastructure;
using ABCLibrary.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ABCLibrary.Wpf;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddInfrastructure(DbBootstrapper.DefaultDbPath);
        serviceCollection.AddTransient<ViewModels.LoginViewModel>();
        serviceCollection.AddTransient<ViewModels.MainViewModel>();
        serviceCollection.AddTransient<Views.LoginWindow>();
        serviceCollection.AddTransient<Views.MainWindow>();

        Services = serviceCollection.BuildServiceProvider();

        var dbContext = Services.GetRequiredService<LibraryDbContext>();
        await DbBootstrapper.InitializeAsync(dbContext);

        var loginWindow = Services.GetRequiredService<Views.LoginWindow>();
        loginWindow.Show();
    }
}
