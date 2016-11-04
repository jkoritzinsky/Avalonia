using System;
using Avalonia.Controls;
using Avalonia.Shared.PlatformSupport;

namespace Avalonia.OSX
{
	public class AppBuilder : AppBuilderBase<AppBuilder>
	{
		public AppBuilder()
			:base(new StandardRuntimePlatform(), () => StandardRuntimePlatformServices.Register())
		{
		}
	}
}
