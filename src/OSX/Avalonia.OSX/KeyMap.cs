using System;
using System.Collections.Generic;
using AppKit;
using Avalonia.Input;

namespace Avalonia.OSX
{
	public static class KeyMap
	{
		public static readonly IReadOnlyDictionary<ushort, Key> KeyCodeMap = new Dictionary<ushort, Key>
		{
			{0x00, Key.A},
	        {0x01, Key.S},
	    	{0x02, Key.D},
			{0x03, Key.F},
			{0x04, Key.H},
			{0x05, Key.G},
			{0x06, Key.Z},
			{0x07, Key.X},
			{0x08, Key.C},
			{0x09, Key.V},
			{0x0B, Key.B},
			{0x0C, Key.Q},
			{0x0D, Key.W},
			{0x0E, Key.E},
			{0x0F, Key.R},
			{0x10, Key.Y},
			{0x11, Key.T},
			{0x12, Key.D1},
			{0x13, Key.D2},
			{0x14, Key.D3},
			{0x15, Key.D4},
			{0x16, Key.D6},
			{0x17, Key.D5},
			{0x18, Key.OemPlus},
			{0x19, Key.D9},
			{0x1A, Key.D7},
			{0x1B, Key.OemMinus},
			{0x1C, Key.D8},
			{0x1D, Key.D0},
			{0x1E, Key.OemCloseBrackets},
			{0x1F, Key.O},
			{0x20, Key.U},
			{0x21, Key.OemOpenBrackets},
			{0x22, Key.I},
			{0x23, Key.P},
			{0x25, Key.L},
			{0x26, Key.J},
			{0x27, Key.OemQuotes},
			{0x28, Key.K},
			{0x29, Key.OemSemicolon},
			{0x2A, Key.OemBackslash},
			{0x2B, Key.OemComma},
			{0x2C, Key.OemPipe},
			{0x2D, Key.N},
			{0x2E, Key.M},
			{0x2F, Key.OemPeriod},
			{0x32, Key.OemTilde},
			{0x41, Key.Decimal},
			{0x43, Key.Multiply},
			{0x45, Key.Add},
			{0x47, Key.Clear}, // Clear on the NumPad
			{0x4B, Key.Divide},
			{0x4C, Key.Separator},
			{0x4E, Key.Subtract},
			{0x51, Key.OemPlus}, // = on the NumPad
			{0x52, Key.NumPad0},
			{0x53, Key.NumPad1},
			{0x54, Key.NumPad2},
			{0x55, Key.NumPad3},
			{0x56, Key.NumPad4},
			{0x57, Key.NumPad5},
			{0x58, Key.NumPad6},
			{0x59, Key.NumPad7},
			{0x5B, Key.NumPad8},
			{0x5C, Key.NumPad9},
			{0x24, Key.Return},
			{0x30, Key.Tab},
			{0x31, Key.Space},
			{0x33, Key.Delete},
			{0x35, Key.Escape},
			{0x37, Key.LWin},
			{0x38, Key.LeftShift},
			{0x39, Key.CapsLock},
			{0x3A, Key.LWin},
			{0x3B, Key.LeftCtrl},
			{0x3C, Key.RightShift},
			{0x3D, Key.RWin},
			{0x3E, Key.RightCtrl},
			// {0x3F, Key.Function},
			{0x40, Key.F17},
			{0x48, Key.VolumeUp},
			{0x49, Key.VolumeDown},
			// {0x4A, Key.Mute},
			{0x4F, Key.F18},
			{0x50, Key.F19},
			{0x5A, Key.F20},
			{0x60, Key.F5},
			{0x61, Key.F6},
			{0x62, Key.F7},
			{0x63, Key.F3},
			{0x64, Key.F8},
			{0x65, Key.F9},
			{0x67, Key.F11},
			{0x69, Key.F13},
			{0x6A, Key.F16},
			{0x6B, Key.F14},
			{0x6D, Key.F10},
			{0x6F, Key.F12},
			{0x71, Key.F15},
			{0x72, Key.Help},
			{0x73, Key.Home},
			{0x74, Key.PageUp},
			{0x75, Key.Delete},
			{0x76, Key.F4},
			{0x77, Key.End},
			{0x78, Key.F2},
			{0x79, Key.PageDown},
			{0x7A, Key.F1},
			{0x7B, Key.Left},
			{0x7C, Key.Right},
			{0x7D, Key.Down},
			{0x7E, Key.Up}
		};

		public static InputModifiers ToAvalonia(this NSEventModifierMask mask)
		{
			InputModifiers retVal = InputModifiers.None;
			if ((mask & NSEventModifierMask.HelpKeyMask) != 0)
			{
			}
			if ((mask & NSEventModifierMask.AlphaShiftKeyMask) != 0)
			{
			}
			if ((mask & NSEventModifierMask.AlternateKeyMask) != 0)
			{
				retVal |= InputModifiers.Alt;
			}
			if ((mask & NSEventModifierMask.CommandKeyMask) != 0)
			{
				retVal |= InputModifiers.Windows;
			}
			if ((mask & NSEventModifierMask.ControlKeyMask) != 0)
			{
				retVal |= InputModifiers.Control;
			}
			if ((mask & NSEventModifierMask.DeviceIndependentModifierFlagsMask) != 0)
			{
			}
			if ((mask & NSEventModifierMask.FunctionKeyMask) != 0)
			{
			}
			if ((mask & NSEventModifierMask.NumericPadKeyMask) != 0)
			{
			}
			if ((mask & NSEventModifierMask.ShiftKeyMask) != 0)
			{
				retVal |= InputModifiers.Shift;
			}
			return retVal;
		}
	}
}
