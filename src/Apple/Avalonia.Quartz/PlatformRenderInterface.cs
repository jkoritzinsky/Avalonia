using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Platform;
namespace Avalonia.Quartz
{
	class PlatformRenderInterface : IPlatformRenderInterface
	{
		public IFormattedTextImpl CreateFormattedText(string text, string fontFamilyName, double fontSize, FontStyle fontStyle, TextAlignment textAlignment, FontWeight fontWeight, TextWrapping wrapping)
		{
			return new FormattedTextImpl(text, fontFamilyName, fontSize, fontStyle, textAlignment, fontWeight, wrapping);
		}

		public IRenderTarget CreateRenderTarget(IPlatformHandle handle)
		{
			return new RenderTarget(handle);
		}

		public IRenderTargetBitmapImpl CreateRenderTargetBitmap(int width, int height)
		{
			return new RenderTargetBitmapImpl(width, height);
		}

		public IStreamGeometryImpl CreateStreamGeometry()
		{
			return new StreamGeometryImpl(new CoreGraphics.CGPath());
		}

		public IBitmapImpl LoadBitmap(Stream stream)
		{
			return new BitmapImpl(stream);
		}

		public IBitmapImpl LoadBitmap(string fileName)
		{
			return new BitmapImpl(fileName);
		}
	}
}
