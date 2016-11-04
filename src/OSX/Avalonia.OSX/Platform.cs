using System;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Avalonia
{
	public static class OSXAppBuilderExtensions
	{
		public static TAppBuilder UseAppKit<TAppBuilder>(this TAppBuilder builder)
			where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
		{
			return builder.UseWindowingSubsystem(Avalonia.OSX.Platform.Initialize, "AppKit");
		}
	}
}

namespace Avalonia.OSX
{
	public static class Platform
	{
		public static void Initialize()
		{
			AvaloniaLocator.CurrentMutable
			               .Bind<IPlatformThreadingInterface>().ToSingleton<PlatformThreadingInterface>()
			               .Bind<IWindowingPlatform>().ToSingleton<WindowingPlatform>()
			               .Bind<IPlatformIconLoader>().ToSingleton<PlatformIconLoader>();
		}
	}
}
