using System;
using AppKit;
using Avalonia.Input;
using Avalonia.Input.Raw;

namespace Avalonia.OSX
{
	class NSAvaloniaWindow : NSWindow
	{
		private NSEventModifierMask previousModifiers = 0;
		public IInputRoot InputRoot { get; set; }
		public Action<RawInputEventArgs> InputCallback { get; set; }

		public NSAvaloniaWindow(Action<RawInputEventArgs> inputCallback)
		{
			InputCallback = inputCallback;
			AcceptsMouseMovedEvents = true;
		}

		public override void FlagsChanged(NSEvent theEvent)
		{
			base.FlagsChanged(theEvent);
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
			base.KeyUp(theEvent);
			InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
									  (uint)theEvent.Timestamp,
									  RawKeyEventType.KeyUp,
									  KeyMap.KeyCodeMap[theEvent.KeyCode],
			                          theEvent.ModifierFlags.ToAvalonia()));
		}

		public override void KeyDown(NSEvent theEvent)
		{
			base.KeyDown(theEvent);
			InputCallback?.Invoke(new RawKeyEventArgs(KeyboardDevice.Instance,
									  (uint)theEvent.Timestamp,
						              RawKeyEventType.KeyDown,
									  KeyMap.KeyCodeMap[theEvent.KeyCode],
									  theEvent.ModifierFlags.ToAvalonia()));
		}

		public override void MouseDown(NSEvent theEvent)
		{
			base.MouseDown(theEvent);
		}

		public override void MouseUp(NSEvent theEvent)
		{
			base.MouseUp(theEvent);
		}

		public override void MouseDragged(NSEvent theEvent)
		{
			base.MouseDragged(theEvent);
		}

		public override void RightMouseDown(NSEvent theEvent)
		{
			base.RightMouseDown(theEvent);
		}

		public override void RightMouseUp(NSEvent theEvent)
		{
			base.RightMouseUp(theEvent);
		}

		public override void RightMouseDragged(NSEvent theEvent)
		{
			base.RightMouseDragged(theEvent);
		}

		public override void OtherMouseUp(NSEvent theEvent)
		{
			base.OtherMouseUp(theEvent);
		}

		public override void OtherMouseDown(NSEvent theEvent)
		{
			base.OtherMouseDown(theEvent);
		}

		public override void OtherMouseDragged(NSEvent theEvent)
		{
			base.OtherMouseDragged(theEvent);
		}

		public override void MouseMoved(NSEvent theEvent)
		{
			base.MouseMoved(theEvent);
		}

		public override void MouseEntered(NSEvent theEvent)
		{
			base.MouseEntered(theEvent);
		}

		public override void MouseExited(NSEvent theEvent)
		{
			base.MouseExited(theEvent);
		}
	}
}
