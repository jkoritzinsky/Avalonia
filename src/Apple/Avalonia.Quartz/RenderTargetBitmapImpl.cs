using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Platform;
using CoreGraphics;

namespace Avalonia.Quartz
{
	class RenderTargetBitmapImpl : IBitmapImpl, IRenderTargetBitmapImpl
	{
		private CGBitmapContext context;
		private CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

		public RenderTargetBitmapImpl(int width, int height)
		{
			PixelHeight = height;
			PixelWidth = width;
			context = new CGBitmapContext(IntPtr.Zero, PixelWidth, PixelHeight, 8, 0, colorSpace, CGImageAlphaInfo.PremultipliedFirst);
		}

		public int PixelHeight { get; }

		public int PixelWidth { get; }

		public DrawingContext CreateDrawingContext() => new DrawingContext(new DrawingContextImpl(context, new Rect(0, 0, PixelWidth, PixelHeight).ToNative()));

		public void Dispose()
		{
			context?.Dispose();
		}

		public void Save(Stream stream)
		{
			using (var bitmap = new BitmapImpl(context.ToImage()))
			{
				bitmap.Save(stream);
			}
		}

		public void Save(string fileName)
		{
			using (var bitmap = new BitmapImpl(context.ToImage()))
			{
				bitmap.Save(fileName);
			}
		}
	}
}
