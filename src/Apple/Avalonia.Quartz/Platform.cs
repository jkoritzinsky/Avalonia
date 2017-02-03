using System;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Avalonia
{
	public static class QuartzAppBuilderExtensions
	{
		public static TAppBuilder UseQuartz<TAppBuilder>(this TAppBuilder builder)
			where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
		{
			return builder.UseRenderingSubsystem(() => Quartz.Platform.Initialize(), "Quartz");
		}
	}
}

namespace Avalonia.Quartz
{
	public static class Platform
	{
		public static void Initialize()
		{
			AvaloniaLocator.CurrentMutable.Bind<IPlatformRenderInterface>().ToSingleton<PlatformRenderInterface>();
		}
	}
}
