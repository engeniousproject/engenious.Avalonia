using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Keys = engenious.Input.Keys;
using KeyModifiers = OpenTK.Windowing.GraphicsLibraryFramework.KeyModifiers;
using MouseButton = engenious.Input.MouseButton;
using TextInputEventArgs = OpenTK.Windowing.Common.TextInputEventArgs;
using WindowIcon = OpenTK.Windowing.Common.Input.WindowIcon;
using WindowState = OpenTK.Windowing.Common.WindowState;

namespace engenious.Avalonia
{
    public class AvaloniaControlWrapper : IWindowWrapper
    {
        private readonly global::Avalonia.Controls.Window _window;
        private readonly Control _control;
        private readonly IDisposable? _boundsSubscription;

        private static TopLevel GetTopLevelControl(global::Avalonia.Controls.IControl control)
        {
            var curControl = control.Parent;
            TopLevel? topLevel = null;
            while ((topLevel = curControl as TopLevel) == null)
            {
                curControl = curControl.Parent;
            }

            return topLevel;
        }

        public AvaloniaControlWrapper(Control control)
        {
            _control = control;
            var topLevel = GetTopLevelControl(control.Parent);
            _window = topLevel as global::Avalonia.Controls.Window ?? throw new ArgumentException($"Control {nameof(control)} has no toplevel parent");
            

            control.PointerWheelChanged += (sender, args) =>
                MouseWheel?.Invoke(new MouseWheelEventArgs((float) args.Delta.X, (float) args.Delta.Y));
            control.KeyDown += (sender, args) =>
            {
                var mappedKey = AvaloniaKeyMap.Map[(int) args.Key];

                LastKeyboardState = KeyboardState;
                KeyboardState.SetKeyState(mappedKey, true);
            };
            control.KeyUp += (sender, args) =>
            {
                var mappedKey = AvaloniaKeyMap.Map[(int) args.Key];
                
                LastKeyboardState = KeyboardState;
                KeyboardState.SetKeyState(mappedKey, false);
                
            };
            control.TextInput += (sender, args) =>
            {
                if (args.Text == null)
                    return;
                foreach(var c in args.Text)
                   KeyPress?.Invoke(new TextInputEventArgs(c));
            };
            //
            // control.PointerEnter += (sender, args) => MouseEnter?.Invoke();
            // control.PointerLeave += (sender, args) => MouseLeave?.Invoke();

            control.PointerMoved += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);

                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState.X = (int)cursorPos.Position.X;
                tmpMouseState.Y = (int) cursorPos.Position.Y;
                MouseState = tmpMouseState;
                //
                // MouseMove?.Invoke(new MouseMoveEventArgs(tmpMouseState.Position.X, tmpMouseState.Position.Y, 0,
                //     0));
            };
            control.PointerPressed += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);
                var mouseButton = AvaloniaKeyMap.MapMouseButton(cursorPos.Properties.PointerUpdateKind);
                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState[mouseButton] = true;
                // MouseState = tmpMouseState;
                // MouseDown?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Press, 0));
            };
            control.PointerReleased += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);
                var mouseButton = AvaloniaKeyMap.MapMouseButton(cursorPos.Properties.PointerUpdateKind);
                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState[mouseButton] = false;
                // MouseState = tmpMouseState;
                // MouseUp?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Release, 0));
            };
            //control.PointerWheelChanged += (sender, args) => MouseWheel?.Invoke(new MouseWheelEventArgs(args.))

            control.GotFocus += (sender, args) => FocusedChanged?.Invoke(new FocusedChangedEventArgs(true));
            control.LostFocus += (sender, args) => FocusedChanged?.Invoke(new FocusedChangedEventArgs(false));
            
            _boundsSubscription = control.GetObservable(Visual.BoundsProperty).Subscribe(args =>
            {
                var oldBounds = control.Bounds;
                var newBounds = args;


                // if (newBounds.Position != oldBounds.Position)
                // {
                //     Move?.Invoke(new WindowPositionEventArgs((int) newBounds.Position.X, (int) newBounds.Position.Y));
                // }

                if (newBounds.Size != oldBounds.Size)
                {
                    Resize?.Invoke(new ResizeEventArgs((int) newBounds.Size.Width, (int) newBounds.Size.Height));
                }
            });
            _window.Closing += (sender, args) =>
            {
                var c = new CancelEventArgs(false);
                Closing?.Invoke(c);
                args.Cancel = c.Cancel;
            };

            // _windowStateSubscription = _window.GetObservable(global::Avalonia.Controls.Window.WindowStateProperty).Subscribe(args =>
            // {
            //     switch (args)
            //     {
            //         case global::Avalonia.Controls.WindowState.Minimized:
            //             Minimized?.Invoke(new MinimizedEventArgs(true));
            //             break;
            //         default:
            //             Minimized?.Invoke(new MinimizedEventArgs(false));
            //             break;
            //     }
            // });

            if (topLevel is global::Avalonia.Controls.Window window)
                window.Closing += (sender, args) =>
                {
                    var c = new CancelEventArgs(args.Cancel);
                    Closing?.Invoke(c);
                    args.Cancel = c.Cancel;
                };

        }

        public void Dispose()
        {
            _boundsSubscription?.Dispose();
        }

        public void Run()
        {
            
        }

        public void Close()
        {
            _window.Close();
        }

        public Point PointToClient(Point point)
        {
            var res = _control.PointToClient(new PixelPoint(point.X, point.Y));
            return new Point((int) res.X, (int) res.Y);
        }

        public Point PointToScreen(Point point)
        {
            var res = _control.PointToScreen(new global::Avalonia.Point(point.X, point.Y));
            return new Point(res.X, res.Y);
        }
        public IGraphicsContext Context => throw new NotImplementedException();
        public WindowIcon? Icon { get; set; }

        public string Title
        {
            get => _window.Title;
            set => _window.Title = value;
        }
        public bool IsFocused { get; set; }
        public bool IsVisible { get; set; }
        public WindowState WindowState { get; set; }
        public WindowBorder WindowBorder { get; set; }
        public Box2i Bounds { get; set; }
        public Point Location { get; set; }
        public Point Size { get; set; }
        public Rectangle ClientRectangle { get; set; }
        public Point ClientSize => new Point((int)_control.Bounds.Width, (int)_control.Bounds.Height);
        public bool CursorVisible { get; set; }
        public bool CursorGrabbed { get; set; }
        public IReadOnlyList<JoystickState?> JoystickStates => Array.Empty<JoystickState>();
        public IReadOnlyList<JoystickState?> LastJoystickStates => Array.Empty<JoystickState>();

        public engenious.Input.KeyboardState KeyboardState { get; private set; }

        public engenious.Input.KeyboardState LastKeyboardState{ get; private set; }
        public Vector2 MousePosition { get; set; }

        public engenious.Input.MouseState MouseState{ get; private set; }

        public engenious.Input.MouseState LastMouseState { get; private set; }
        public event Action<ResizeEventArgs>? Resize;


        public event Action? Load;
        public event Action<FrameEventArgs>? UpdateFrame;
        public event Action<CancelEventArgs>? Closing;
        public event Action<FocusedChangedEventArgs>? FocusedChanged;
        public event Action<FrameEventArgs>? RenderFrame;
        public event Action<TextInputEventArgs>? KeyPress;
        public event Action<MouseWheelEventArgs>? MouseWheel;
    }
}