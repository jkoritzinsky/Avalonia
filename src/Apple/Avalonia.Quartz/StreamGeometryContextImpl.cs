using System;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.RenderHelpers;
using CoreGraphics;

namespace Avalonia.Quartz
{
	class StreamGeometryContextImpl : IStreamGeometryContextImpl
	{
		private StreamGeometryImpl impl;

		public StreamGeometryContextImpl(StreamGeometryImpl impl)
		{
			this.impl = impl;
		}

		public void ArcTo(Point point, Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection)
		{
			ArcToHelper.ArcTo(this, impl.Path.CurrentPoint.ToAvalonia(), point, size, rotationAngle, isLargeArc, sweepDirection);
		}

		public void BeginFigure(Point startPoint, bool isFilled)
		{
			impl.Path.MoveToPoint(startPoint.ToNative());
		}

		public void CubicBezierTo(Point point1, Point point2, Point point3)
		{
			impl.Path.AddCurveToPoint(point1.ToNative(), point2.ToNative(), point3.ToNative());
		}

		public void Dispose()
		{
		}

		public void EndFigure(bool isClosed)
		{
			if (isClosed)
			{
				impl.Path.CloseSubpath();
			}
		}

		public void LineTo(Point point)
		{
			impl.Path.AddLineToPoint(point.ToNative());
		}

		public void QuadraticBezierTo(Point control, Point endPoint)
		{
			impl.Path.AddQuadCurveToPoint((nfloat)control.X, (nfloat)control.Y, (nfloat)endPoint.X, (nfloat)endPoint.Y);
		}

		public void SetFillRule(FillRule fillRule)
		{
			impl.Fill = fillRule;
		}
	}
}
