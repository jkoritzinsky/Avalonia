using System;
using System.Reactive.Disposables;
using AppKit;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Platform;

namespace Avalonia.OSX
{
	public class WindowImpl : NSWindowDelegate, IWindowImpl
	{
		private const NSWindowStyle WindowDecorations = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Miniaturizable;
		private readonly NSWindow window;

		public WindowImpl()
		{
			window = new NSWindow();
			window.StyleMask |= WindowDecorations;
			window.Delegate = this;
		}

		public override void DidBecomeKey(Foundation.NSNotification notification)
		{
			Activated?.Invoke();
		}

		public override void DidBecomeMain(Foundation.NSNotification notification)
		{
			Activated?.Invoke();
		}

		public Action Activated { get; set; }

		public Size ClientSize
		{
			get
			{
				return window.ContentLayoutRect.Size.ToAvalonia();
			}

			set
			{
				window.SetContentSize(value.ToNative());
			}
		}

		public override void WillClose(Foundation.NSNotification notification)
		{
			Closed?.Invoke();
		}

		public Action Closed { get; set; }

		public override void DidResignKey(Foundation.NSNotification notification)
		{
			Deactivated?.Invoke();
		}

		public override void DidResignMain(Foundation.NSNotification notification)
		{
			Deactivated?.Invoke();
		}

		public Action Deactivated { get; set; }

		public new IPlatformHandle Handle => new PlatformHandle(window.WindowRef, "NSWindow");

		public Action<RawInputEventArgs> Input { get; set; }

		public Size MaxClientSize
		{
			get
			{
				return window.ContentMaxSize.ToAvalonia();
			}
		}

		public Action<Rect> Paint { get; set; }

		public Point Position
		{
			get
			{
				return new Point(window.Frame.Left, window.Frame.Top);
			}

			set
			{
				window.SetFrameTopLeftPoint(value.ToNative());
			}
		}

		public Action<Point> PositionChanged { get; set; }

		public override void DidMove(Foundation.NSNotification notification)
		{
			PositionChanged?.Invoke(Position);
		}

		public Action<Size> Resized { get; set; }

		public override void DidResize(Foundation.NSNotification notification)
		{
			Resized?.Invoke(ClientSize);
		}

		public double Scaling
		{
			get
			{
				return 1.0;
			}
		}

		public Action<double> ScalingChanged { get; set; }

		public WindowState WindowState
		{
			get
			{
				return WindowState.Normal;
			}

			set
			{}
		}

		public virtual void Activate()
		{
			window.MakeMainWindow();
		}

		public void BeginMoveDrag()
		{
			
		}

		public void BeginResizeDrag(WindowEdge edge)
		{
			
		}

		public new void Dispose()
		{
			base.Dispose();
			window.Dispose();
		}

		public void Hide()
		{
			window.IsVisible = false;
		}

		public void Invalidate(Rect rect)
		{
			window.ContentView.NeedsToDraw(rect.ToNative());
		}

		public Point PointToClient(Point point)
		{
			return window.ConvertRectFromScreen(new CoreGraphics.CGRect(point.ToNative(), new CoreGraphics.CGSize())).Location.ToAvalonia();
		}

		public Point PointToScreen(Point point)
		{
			return window.ConvertRectToScreen(new CoreGraphics.CGRect(point.ToNative(), new CoreGraphics.CGSize())).Location.ToAvalonia();
		}

		public void SetCursor(IPlatformHandle cursor)
		{
			
		}

		public void SetIcon(IWindowIconImpl icon)
		{
			
		}

		public void SetInputRoot(IInputRoot inputRoot)
		{
			
		}

		public void SetSystemDecorations(bool enabled)
		{
			if (enabled)
			{
				window.StyleMask |= WindowDecorations;
			}
			else
			{
				window.StyleMask &= ~WindowDecorations;
			}
		}

		public void SetTitle(string title)
		{
			window.Title = title;
		}

		public void Show()
		{
			window.IsVisible = true;
			window.MakeMainWindow();
		}

		public IDisposable ShowDialog()
		{
			window.MakeKeyAndOrderFront(this);
			return Disposable.Empty;
		}
	}
}

