using System;
using AppKit;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Foundation;

namespace Avalonia.OSX
{
	class NSAvaloniaWindow : NSWindow
	{
		private NSEventModifierMask previousModifiers = 0;
		public IInputRoot InputRoot { get; set; }
		public Action<RawInputEventArgs> InputCallback { get; }
		public Action UpdateCallback { get; }


		public NSAvaloniaWindow(Action<RawInputEventArgs> inputCallback, Action updateCallback)
		{
			InputCallback = inputCallback;
			UpdateCallback = updateCallback;
			AcceptsMouseMovedEvents = true;
		}

		public override void FlagsChanged(NSEvent theEvent)
		{
			var pressedModifiers = ~previousModifiers & theEvent.ModifierFlags;
			var releasedModifiers = previousModifiers & theEvent.ModifierFlags;
			if (pressedModifiers != 0)
			{
				InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
						  (uint)theEvent.Timestamp,
						  RawKeyEventType.KeyDown,
						  KeyMap.KeyCodeMap[theEvent.KeyCode],
						  theEvent.ModifierFlags.ToAvalonia()));
			}
			else if (releasedModifiers != 0)
			{
				InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
						  (uint)theEvent.Timestamp,
						  RawKeyEventType.KeyUp,
						  KeyMap.KeyCodeMap[theEvent.KeyCode],
						  theEvent.ModifierFlags.ToAvalonia()));
			}
			previousModifiers = theEvent.ModifierFlags;
		}

		public override void KeyUp(NSEvent theEvent)
		{
			InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
									  (uint)theEvent.Timestamp,
									  RawKeyEventType.KeyUp,
									  KeyMap.KeyCodeMap[theEvent.KeyCode],
			                          theEvent.ModifierFlags.ToAvalonia()));
		}

		public override void KeyDown(NSEvent theEvent)
		{
			InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
									  (uint)theEvent.Timestamp,
						              RawKeyEventType.KeyDown,
									  KeyMap.KeyCodeMap[theEvent.KeyCode],
									  theEvent.ModifierFlags.ToAvalonia()));
			InputCallback?.Invoke(new RawTextInputEventArgs(KeyboardDevice.Instance,
									  (uint)theEvent.Timestamp,
									  theEvent.Characters));
		}

		public override void MouseDown(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.LeftButtonDown,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void MouseUp(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.LeftButtonUp,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void RightMouseDown(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.RightButtonDown,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void RightMouseUp(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.RightButtonUp,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void OtherMouseUp(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.MiddleButtonUp,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void OtherMouseDown(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.MiddleButtonDown,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void MouseMoved(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
										RawMouseEventType.Move,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void MouseExited(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
				                        RawMouseEventType.LeaveWindow,
										theEvent.LocationInWindow.ToAvalonia(),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void ScrollWheel(NSEvent theEvent)
		{
			if (InputRoot != null)
			{
				InputCallback?.Invoke(new RawMouseWheelEventArgs(MouseDevice.Instance,
										(uint)theEvent.Timestamp,
										InputRoot,
				                        theEvent.LocationInWindow.ToAvalonia(),
				                        new Vector(theEvent.ScrollingDeltaX, theEvent.ScrollingDeltaY),
										theEvent.ModifierFlags.ToAvalonia()));
			}
		}

		public override void Update()
		{
			base.Update();
			UpdateCallback?.Invoke();
		}
	}
}
