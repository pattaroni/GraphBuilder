using System;
using Avalonia;

namespace GraphBuilder
{
    class Program
    {
        // Точка входу. До запуску застосунку API Avalonia ще не готове
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Налаштування Avalonia
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
        }
    }
}
