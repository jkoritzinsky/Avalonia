using System;
using Avalonia.Media;
using Avalonia.Platform;
using CoreGraphics;

namespace Avalonia.Quartz
{
	class StreamGeometryImpl : IStreamGeometryImpl
	{
		public StreamGeometryImpl(CGPath path)
		{
			Path = path;
		}

		public CGPath Path { get; }

		public Rect Bounds => Path.BoundingBox.ToAvalonia();

		public Matrix Transform { get; set; }

		public FillRule Fill { get; set; }

		public IStreamGeometryImpl Clone()
		{
			return new StreamGeometryImpl(Path.Copy())
			{
				Transform = Transform,
				Fill = Fill
			};
		}

		public bool FillContains(Point point)
		{
			return Path.ContainsPoint(Transform.ToNative(), point.ToNative(), Fill == FillRule.EvenOdd);
		}

		public Rect GetRenderBounds(double strokeThickness)
		{
			if (Transform.IsIdentity)
			{
				return Path.PathBoundingBox.ToAvalonia().Inflate(strokeThickness);
			}
			else
			{
				using (var transformed = Path.CopyByTransformingPath(Transform.ToNative()))
				{
					return transformed.BoundingBox.ToAvalonia().Inflate(strokeThickness);
				}
			}
		}

		public IStreamGeometryContextImpl Open()
		{
			return new StreamGeometryContextImpl(this);
		}
	}
}
