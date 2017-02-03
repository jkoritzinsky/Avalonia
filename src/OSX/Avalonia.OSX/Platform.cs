using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Rendering;

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
	static class Platform
	{
		public static void Initialize()
		{
			AvaloniaLocator.CurrentMutable
			               .Bind<IPlatformThreadingInterface>().ToSingleton<PlatformThreadingInterface>()
			               .Bind<IWindowingPlatform>().ToSingleton<WindowingPlatform>()
			               .Bind<IPlatformIconLoader>().ToSingleton<PlatformIconLoader>()
			               .Bind<IKeyboardDevice>().ToSingleton<KeyboardDevice>()
			               .Bind<IMouseDevice>().ToSingleton<MouseDevice>()
			               .Bind<IRenderLoop>().ToConstant(new DefaultRenderLoop(60))
			               .Bind<IRendererFactory>().ToSingleton<RendererFactory>()
			               .Bind<IPlatformSettings>().ToSingleton<PlatformSettings>();
		}
	}
}
