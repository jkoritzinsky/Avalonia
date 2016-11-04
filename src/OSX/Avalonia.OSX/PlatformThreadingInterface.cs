using System;
using System.Reactive.Disposables;
using System.Threading;
using AppKit;
using Avalonia.Platform;
using Foundation;

namespace Avalonia.OSX
{
	public class PlatformThreadingInterface : IPlatformThreadingInterface
	{
		public bool CurrentThreadIsLoopThread => NSThread.Current.IsMainThread;
		public event Action Signaled;

		public void RunLoop(CancellationToken cancellationToken)
		{
			cancellationToken.Register(() => NSApplication.SharedApplication.Terminate(new NSObject()), true);
		}

		public void Signal()
		{
			NSRunLoop.Main.InvokeOnMainThread(() => Signaled?.Invoke());
		}

		public IDisposable StartTimer(TimeSpan interval, Action tick)
		{
			var timer = NSTimer.CreateRepeatingTimer(interval, _ => tick());
			NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Default);
			return Disposable.Create(() =>
			{
				timer.Invalidate();
				timer.Dispose();
			});
		}
	}
}

