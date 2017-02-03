using System;
using Avalonia.Media;
using Avalonia.Platform;

namespace Avalonia.Quartz
{
	public class RenderTarget : IRenderTarget
	{
		public RenderTarget(IPlatformHandle handle)
		{
		}

		public DrawingContext CreateDrawingContext()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
