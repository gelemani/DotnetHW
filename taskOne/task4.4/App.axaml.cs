using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using task4._4.ViewModels;
using task4._4.Views;

namespace task4._4;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var factory = new CreateEntityFactory();
            var mainWindowViewModel = new MainWindowViewModel(factory);
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}