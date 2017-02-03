using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Platform;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;

namespace Avalonia.Quartz
{
	class BitmapImpl : IBitmapImpl, IDisposable
	{
		public BitmapImpl(CGImage image)
		{
			Image = image;
		}

		public BitmapImpl(Stream stream)
		{
			using (var memStream = new MemoryStream())
			{
				stream.CopyTo(memStream);
				using (var provider = new CGDataProvider(memStream.ToArray()))
				using (var source = CGImageSource.FromDataProvider(provider))
				{
					Image = source.CreateImage(0, new CGImageOptions { BestGuessTypeIdentifier = UTType.PNG });
				}
			}
		}

		public BitmapImpl(string fileName)
		{
			using (var provider = new CGDataProvider(fileName))
			{
				Image = CGImage.FromPNG(provider, null, true, CGColorRenderingIntent.Default);
			}
		}

		public CGImage Image { get; }

		public int PixelHeight => (int)Image.Height;

		public int PixelWidth => (int)Image.Width;

		public void Dispose()
		{
			Image?.Dispose();
		}

		public void Save(Stream stream)
		{
			using (var data = new NSMutableData())
			{
				using (var destination = CGImageDestination.Create(data, UTType.PNG, 1))
				{
					destination.AddImage(Image);
				}

				var dataArray = new byte[data.Length];
				Marshal.Copy(data.Bytes, dataArray, 0, (int)data.Length);
				stream.Write(dataArray, 0, dataArray.Length);
			}
		}

		public void Save(string fileName)
		{
			using (var destination = CGImageDestination.Create(new NSUrl(fileName, false), UTType.PNG, 1))
			{
				destination.AddImage(Image);
			}
		}
	}
}
