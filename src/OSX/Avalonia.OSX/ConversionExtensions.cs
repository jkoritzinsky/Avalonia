using System;
using CoreGraphics;

namespace Avalonia.OSX
{
	static class ConversionExtensions
	{
		public static CGSize ToNative(this Size size)
		{
			return new CGSize(size.Width, size.Height);
		}

		public static CGPoint ToNative(this Point point)
		{
			return new CGPoint(point.X, point.Y);
		}

		public static CGRect ToNative(this Rect rect)
		{
			return new CGRect(rect.TopLeft.ToNative(), rect.Size.ToNative());
		}

		public static Size ToAvalonia(this CGSize size)
		{
			return new Size(size.Width, size.Height);
		}

		public static Point ToAvalonia(this CGPoint point)
		{
			return new Point(point.X, point.Y);
		}

		public static Rect ToAvalonia(this CGRect rect)
		{
			return new Rect(rect.X, rect.Y, rect.Width, rect.Height);
		}
	}
}
