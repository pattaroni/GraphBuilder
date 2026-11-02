using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace GraphBuilder
{
    public partial class App : Application
    {
        private Window mainWindow;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IClassicDesktopStyleApplicationLifetime desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (desktop != null)
            {
                desktop.MainWindow = new MainWindow();
                mainWindow = desktop.MainWindow;
            }

            Dispatcher.UIThread.UnhandledException += UIThread_UnhandledException;

            base.OnFrameworkInitializationCompleted();
        }

        // Остання лінія захисту, щоб непередбачена помилка не закрила програму
        private void UIThread_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (mainWindow != null && mainWindow.IsVisible)
            {
                MessageWindow window = new MessageWindow("Помилка", "Сталася непередбачена помилка. Спробуйте ще раз.", false);
                window.ShowDialog(mainWindow);
            }
        }
    }
}
