using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CoreGraphics;

namespace Avalonia.Quartz
{
	class DrawingContextImpl : IDrawingContextImpl
	{
		private CGContext context;
		private Stack<double> opacityStack = new Stack<double>();
		private double currentOpacity = 1.0;

		public DrawingContextImpl(CGContext context, CGRect contentRect)
		{
			this.context = context;
		}

		private Matrix _transform = Matrix.Identity;
		public Matrix Transform
		{
			get { return _transform; }
			set
			{
				context.ConcatCTM(_transform.ToNative().Invert());
				_transform = value;
				var native = value.ToNative();
				context.ConcatCTM(native);
				context.TextMatrix = value.ToNative();
			}
		}

		public void Dispose()
		{
		}

		public void DrawGeometry(IBrush brush, Pen pen, Geometry geometry)
		{
			using (var fill = SetFill(brush))
			using (var stroke = SetStrokePattern(pen.Brush))
			{
				var impl = (StreamGeometryImpl)geometry.PlatformImpl;
				var path = impl.Path;
				using (var strokedPath = path.CopyByStrokingPath(impl.Transform.ToNative(), (nfloat)pen.Thickness, pen.StartLineCap.ToNative(), pen.LineJoin.ToNative(), (nfloat)pen.MiterLimit))
				{
					context.AddPath(strokedPath);
				}
				switch (impl.Fill)
				{
					case FillRule.EvenOdd:
						context.EOFillPath();
						break;
					case FillRule.NonZero:
						context.FillPath();
						break;
				}
				context.StrokePath();
			}
		}

		IDisposable SetFill(IBrush brush)
		{
			context.SaveState();
			var solidColor = brush as ISolidColorBrush;
			if (solidColor != null)
			{
				context.SetFillColor(solidColor.Color.ToNative());
			}
			else throw new NotImplementedException();
			return Disposable.Create(() => context.RestoreState());
		}

		IDisposable SetStroke(Pen pen)
		{
			context.SaveState();
			context.SetLineWidth((nfloat)pen.Thickness);
			context.SetLineCap(pen.StartLineCap.ToNative());
			context.SetLineJoin(pen.LineJoin.ToNative());
			context.SetMiterLimit((nfloat)pen.MiterLimit);
			return new CompositeDisposable(Disposable.Create(() => context.RestoreState()), SetStrokePattern(pen.Brush));
		}

		IDisposable SetStrokePattern(IBrush brush)
		{
			if (brush == null) return Disposable.Empty;
			context.SaveState();
			var solidColor = brush as ISolidColorBrush;
			if (solidColor != null)
			{
				context.SetStrokeColor(solidColor.Color.ToNative());
			}
			else throw new NotImplementedException();
			return Disposable.Create(() => context.RestoreState());
		}

		public void DrawImage(IBitmap source, double opacity, Rect sourceRect, Rect destRect)
		{
			var impl = (BitmapImpl)source.PlatformImpl;
			var sourceImg = impl.Image.WithImageInRect(sourceRect.ToNative());
			PushOpacity(opacity);
			context.DrawImage(destRect.ToNative(), sourceImg);
			PopOpacity();
		}

		public void DrawLine(Pen pen, Point p1, Point p2)
		{
			using (var stroke = SetStroke(pen))
			{
				context.AddLines(new[] { p1.ToNative(), p2.ToNative() });
				context.StrokePath();
			}
		}

		public void DrawRectangle(Pen pen, Rect rect, float cornerRadius = 0)
		{
			using (var stroke = SetStroke(pen))
			{
				if (cornerRadius <= 0)
				{
					context.StrokeRect(rect.ToNative());
				}
				else
				{
					using (var roundedRectanglePath = new CGPath())
					{
						roundedRectanglePath.AddRoundedRect(rect.ToNative(), cornerRadius, cornerRadius);
						context.AddPath(roundedRectanglePath);
						context.StrokePath();
					}
				}
			}
		}

		public void DrawText(IBrush foreground, Point origin, FormattedText text)
		{
			using (var stroke = SetStrokePattern(foreground))
			{
				var clip = context.GetClipBoundingBox().ToAvalonia();
				var textSpace = new Rect(origin, new Size(clip.Right - origin.X, clip.Bottom - origin.Y));
				var impl = (FormattedTextImpl)text.PlatformImpl;
				using (var path = new CGPath())
				{
					path.AddRect(textSpace.ToNative());
					var frame = impl.Framesetter.GetFrame(new Foundation.NSRange(0, text.Text.Length), path, null);
					frame.Draw(context);
				}
			}
		}

		public void FillRectangle(IBrush brush, Rect rect, float cornerRadius = 0)
		{
			using (var fill = SetFill(brush))
			{
				if (cornerRadius <= 0)
				{
					context.FillRect(rect.ToNative());
				}
				else
				{
					using (var roundedRectanglePath = new CGPath())
					{
						roundedRectanglePath.AddRoundedRect(rect.ToNative(), cornerRadius, cornerRadius);
						context.AddPath(roundedRectanglePath);
						context.FillPath();
					}
				}
			}
		}

		public void PopClip()
		{
			context.RestoreState();
		}

		public void PopGeometryClip()
		{
			context.RestoreState();
		}

		public void PopOpacity()
		{
			context.RestoreState();
			currentOpacity /= opacityStack.Pop();
		}

		public void PopOpacityMask()
		{
		}

		public void PushClip(Rect clip)
		{
			context.SaveState();
			context.ClipToRect(clip.ToNative());
		}

		public void PushGeometryClip(Geometry clip)
		{
			var impl = (StreamGeometryImpl)clip.PlatformImpl;
			context.SaveState();
			context.AddPath(impl.Path);
			switch (impl.Fill)
			{
				case FillRule.EvenOdd:
					context.EOClip();
					break;
				case FillRule.NonZero:
					context.Clip();
					break;
			}
		}

		public void PushOpacity(double opacity)
		{
			context.SaveState();
			currentOpacity *= opacity;
			opacityStack.Push(opacity);
			context.SetAlpha((nfloat)currentOpacity);
		}

		public void PushOpacityMask(IBrush mask, Rect bounds)
		{
			// TODO: Implement opacity masks
		}
	}
}
