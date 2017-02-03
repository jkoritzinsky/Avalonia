using System;
using Avalonia.Media;
using CoreGraphics;
using CoreText;

namespace Avalonia.Quartz
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

		public static CGAffineTransform ToNative(this Matrix matrix)
		{
			return new CGAffineTransform((nfloat)matrix.M11, (nfloat)matrix.M21, (nfloat)matrix.M12, (nfloat)matrix.M22, (nfloat)matrix.M31, (nfloat)matrix.M32);
		}

		public static CGLineCap ToNative(this PenLineCap lineCap)
		{
			switch (lineCap)
			{
				case PenLineCap.Round:
					return CGLineCap.Round;
				case PenLineCap.Square:
					return CGLineCap.Square;
				case PenLineCap.Flat:
				case PenLineCap.Triangle: // Quartz does not have a triangle line cap, so fall back to butt
					return CGLineCap.Butt;
				default:
					throw new InvalidOperationException();
			}
		}

		public static CGLineJoin ToNative(this PenLineJoin lineJoin)
		{
			switch (lineJoin)
			{
				case PenLineJoin.Round:
					return CGLineJoin.Round;
				case PenLineJoin.Bevel:
					return CGLineJoin.Bevel;
				case PenLineJoin.Miter:
					return CGLineJoin.Miter;
				default:
					throw new InvalidOperationException();
			}
		}

		public static CTTextAlignment ToNative(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Left:
					return CTTextAlignment.Left;
				case TextAlignment.Center:
					return CTTextAlignment.Center;
				case TextAlignment.Right:
					return CTTextAlignment.Right;
				default:
					return CTTextAlignment.Natural;
			}
		}

		public static nfloat ToNative(this FontWeight weight)
		{
			switch (weight)
			{
				case FontWeight.Thin:
					return -0.8f;
				case FontWeight.UltraLight:
					return -0.5f;
				case FontWeight.Light:
					return -0.3f;
				case FontWeight.Regular:
				case FontWeight.Medium:
					return 0;
				case FontWeight.DemiBold:
					return 0.2f;
				case FontWeight.Bold:
					return 0.4f;
				case FontWeight.ExtraBold:
					return 0.6f;
				case FontWeight.Black:
					return 0.8f;
				case FontWeight.ExtraBlack:
					return 1.0f;
				default:
					throw new InvalidOperationException();
			}
		}

		public static CGColor ToNative(this Color color) => new CGColor(color.R, color.G, color.B, color.A);

		public static CTLineBreakMode ToNative(this TextWrapping wrapping)
		{
			switch (wrapping)
			{
				case TextWrapping.NoWrap:
					return CTLineBreakMode.Clipping;
				case TextWrapping.Wrap:
					return CTLineBreakMode.WordWrapping;
				default:
					throw new InvalidOperationException();
			}
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

		public static Matrix ToAvalonia(this CGAffineTransform transform)
		{
			return new Matrix(transform.xx, transform.xy, transform.yx, transform.yy, transform.x0, transform.y0);
		}
	}
}
