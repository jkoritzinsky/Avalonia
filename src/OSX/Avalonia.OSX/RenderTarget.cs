using System;
using Avalonia.Media;
using Avalonia.Platform;
using AppKit;

namespace Avalonia.Quartz
{
	class RenderTarget : IRenderTarget
	{
		private NSWindow window;
		public RenderTarget(IPlatformHandle handle)
		{
			if (handle.HandleDescriptor != "NSWindow")
				throw new InvalidOperationException();
			window = ObjCRuntime.Runtime.GetNSObject<NSWindow>(handle.Handle);
		}

		public DrawingContext CreateDrawingContext()
		{
			return new DrawingContext(new DrawingContextImpl(window.GraphicsContext.CGContext, window.ContentLayoutRect));
		}

		public void Dispose()
		{
		}
	}
}
