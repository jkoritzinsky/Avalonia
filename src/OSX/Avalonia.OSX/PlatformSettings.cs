using System;
using Avalonia.Platform;
using AppKit;

namespace Avalonia.OSX
{
	class PlatformSettings : IPlatformSettings
	{
		public Size DoubleClickSize => new Size(4, 4);

		public TimeSpan DoubleClickTime => TimeSpan.FromSeconds(NSEvent.DoubleClickInterval);
	}
}
