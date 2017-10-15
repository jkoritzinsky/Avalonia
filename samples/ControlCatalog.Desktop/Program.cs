using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.Platform;
using Serilog;

namespace ControlCatalog
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>();
        }

        /// <summary>
        /// This method is needed for IDE previewer infrastructure
        /// </summary>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
            .EnableSerilogLogging(
                Avalonia.Logging.LogEventLevel.Warning,
                config => config.WriteTo.Trace(outputTemplate: "{Area}: {Message}"))
            .UsePlatformDetect();

        private static void ConfigureAssetAssembly(AppBuilder builder)
        {
            AvaloniaLocator.CurrentMutable
                .GetService<IAssetLoader>()
                .SetDefaultAssembly(typeof(App).Assembly);
        }
    }
}
