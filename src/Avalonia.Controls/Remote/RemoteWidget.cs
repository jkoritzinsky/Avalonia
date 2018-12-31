using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Input;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using PixelFormat = Avalonia.Platform.PixelFormat;

namespace Avalonia.Controls.Remote
{
    public class RemoteWidget : Control
    {
        public enum SizingMode
        {
            Local,
            Remote
        }

        private readonly IAvaloniaRemoteTransportConnection _connection;
        private FrameMessage _lastFrame;
        private WriteableBitmap _bitmap;
        public RemoteWidget(IAvaloniaRemoteTransportConnection connection)
        {
            Mode = SizingMode.Local;

            _connection = connection;
            _connection.OnMessage += (t, msg) => Dispatcher.UIThread.Post(() => OnMessage(msg));
            _connection.Send(new ClientSupportedPixelFormatsMessage
            {
                Formats = new[]
                {
                    Avalonia.Remote.Protocol.Viewport.PixelFormat.Bgra8888,
                    Avalonia.Remote.Protocol.Viewport.PixelFormat.Rgba8888,
                }
            });

            AddHandler(KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel);
            AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            AddHandler(TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
        }

        public SizingMode Mode { get; set; }

        private void OnMessage(object msg)
        {
            if (msg is FrameMessage frame)
            {
                _connection.Send(new FrameReceivedMessage
                {
                    SequenceId = frame.SequenceId
                });
                _lastFrame = frame;
                InvalidateVisual();
            }
            
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            if (Mode == SizingMode.Local)
            {
                _connection.Send(new ClientViewportAllocatedMessage
                {
                    Width = finalRect.Width,
                    Height = finalRect.Height,
                    DpiX = 10 * 96,
                    DpiY = 10 * 96 //TODO: Somehow detect the actual DPI
                });
            }

            base.ArrangeCore(finalRect);
        }

        public override void Render(DrawingContext context)
        {
            if (_lastFrame != null)
            {
                var fmt = (PixelFormat) _lastFrame.Format;
                if (_bitmap == null || _bitmap.PixelSize.Width != _lastFrame.Width ||
                    _bitmap.PixelSize.Height != _lastFrame.Height)
                    _bitmap = new WriteableBitmap(new PixelSize(_lastFrame.Width, _lastFrame.Height), new Vector(96, 96), fmt);
                using (var l = _bitmap.Lock())
                {
                    var lineLen = (fmt == PixelFormat.Rgb565 ? 2 : 4) * _lastFrame.Width;
                    for (var y = 0; y < _lastFrame.Height; y++)
                        Marshal.Copy(_lastFrame.Data, y * _lastFrame.Stride,
                            new IntPtr(l.Address.ToInt64() + l.RowBytes * y), lineLen);
                }
                context.DrawImage(_bitmap, 1, new Rect(0, 0, _bitmap.PixelSize.Width, _bitmap.PixelSize.Height),
                    new Rect(Bounds.Size));
            }
            base.Render(context);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var point = e.GetPosition(this);

            _connection.Send(new PointerPressedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                Button = (Avalonia.Remote.Protocol.Input.MouseButton)e.MouseButton
            });

            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            var point = e.GetPosition(this);

            _connection.Send(new PointerReleasedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                Button = (Avalonia.Remote.Protocol.Input.MouseButton)e.MouseButton
            });

            base.OnPointerReleased(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var point = e.GetPosition(this);

            _connection.Send(new ScrollEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                DeltaX = e.Delta.X,
                DeltaY = e.Delta.Y
            });

            base.OnPointerWheelChanged(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var point = e.GetPosition(this);

            _connection.Send(new PointerMovedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y
            });

            base.OnPointerMoved(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _connection.Send(new KeyEventMessage
            {
                IsDown = true,
                Key = (Avalonia.Remote.Protocol.Input.Key)e.Key,
                Modifiers = ToAvaloniaModifiers(e.Modifiers)
            });

            e.Handled = true;

            base.OnKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _connection.Send(new KeyEventMessage
            {
                IsDown = false,
                Key = (Avalonia.Remote.Protocol.Input.Key)e.Key,
                Modifiers = ToAvaloniaModifiers(e.Modifiers)
            });

            e.Handled = true;

            base.OnKeyUp(e);
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            _connection.Send(new TextInputEventMessage
            {
                Text = e.Text
            });

            e.Handled = true;

            base.OnTextInput(e);
        }

        private static Avalonia.Remote.Protocol.Input.InputModifiers[] ToAvaloniaModifiers(Avalonia.Input.InputModifiers modifiers)
        {
            var result = new List<Avalonia.Remote.Protocol.Input.InputModifiers>();

            if ((modifiers & Avalonia.Input.InputModifiers.Control) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Control);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.Alt) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Alt);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.Shift) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Shift);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.Windows) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Windows);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.LeftMouseButton) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.LeftMouseButton);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.RightMouseButton) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.RightMouseButton);
            }

            if ((modifiers & Avalonia.Input.InputModifiers.MiddleMouseButton) != 0)
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.MiddleMouseButton);
            }

            return result.ToArray();
        }
    }
}
