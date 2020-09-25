using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using Key = OpenTK.Windowing.Common.Input.Key;
using KeyModifiers = OpenTK.Windowing.Common.Input.KeyModifiers;
using MouseButton = OpenTK.Windowing.Common.Input.MouseButton;
using TextInputEventArgs = OpenTK.Windowing.Common.TextInputEventArgs;
using WindowIcon = OpenTK.Windowing.Common.Input.WindowIcon;
using WindowState = OpenTK.Windowing.Common.WindowState;

namespace engenious.Avalonia
{
    public class AvaloniaControlWrapper : INativeWindow
    {
        private readonly global::Avalonia.Controls.Window _window;
        private readonly global::Avalonia.Controls.TopLevel _topLevel;
        private readonly Control _control;
        private readonly IDisposable _boundsSubscription, _windowStateSubscription;

        private static TopLevel GetTopLevelControl(global::Avalonia.Controls.IControl control)
        {
            var curControl = control.Parent;
            TopLevel topLevel = null;
            while ((topLevel = curControl as TopLevel) == null)
            {
                curControl = curControl.Parent;
            }

            return topLevel;
        }

        public AvaloniaControlWrapper(Control control)
        {
            _topLevel = GetTopLevelControl(control.Parent);
            if (_topLevel == null)
                throw new ArgumentException($"Control {nameof(control)} has no toplevel parent");
            _window = _topLevel as global::Avalonia.Controls.Window;


            control.KeyDown += (sender, args) =>
            {
                var mappedKey = AvaloniaKeyMap.Map[(int) args.Key];

                LastKeyboardState = KeyboardState;
                KeyboardState.SetKeyState(mappedKey, true);
                
                KeyDown?.Invoke(new KeyboardKeyEventArgs(mappedKey, 0, AvaloniaKeyMap.MapModifier(args.KeyModifiers),
                    false));
            };
            control.KeyUp += (sender, args) =>
            {
                var mappedKey = AvaloniaKeyMap.Map[(int) args.Key];
                
                LastKeyboardState = KeyboardState;
                KeyboardState.SetKeyState(mappedKey, false);
                
                KeyUp?.Invoke(new KeyboardKeyEventArgs(mappedKey, 0, AvaloniaKeyMap.MapModifier(args.KeyModifiers),
                    false));
            };
            control.TextInput += (sender, args) =>
            {
                var unicode = BitConverter.ToInt32(System.Text.Encoding.UTF8.GetBytes(args.Text));
                TextInput?.Invoke(new TextInputEventArgs(unicode));
            };

            control.PointerEnter += (sender, args) => MouseEnter?.Invoke();
            control.PointerLeave += (sender, args) => MouseLeave?.Invoke();

            control.PointerMoved += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);

                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState.Position = new OpenTK.Mathematics.Vector2((float) cursorPos.Position.X,(float) cursorPos.Position.Y);
                MouseState = tmpMouseState;
                
                MouseMove?.Invoke(new MouseMoveEventArgs(tmpMouseState.Position.X, tmpMouseState.Position.Y, 0,
                    0));
            };
            control.PointerPressed += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);
                var mouseButton = AvaloniaKeyMap.MapMouseButton(cursorPos.Properties.PointerUpdateKind);
                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState[mouseButton] = true;
                MouseState = tmpMouseState;
                MouseDown?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Press, 0));
            };
            control.PointerReleased += (sender, args) =>
            {
                var cursorPos = args.GetCurrentPoint(_control);
                var mouseButton = AvaloniaKeyMap.MapMouseButton(cursorPos.Properties.PointerUpdateKind);
                LastMouseState = MouseState;
                var tmpMouseState = MouseState;
                tmpMouseState[mouseButton] = false;
                MouseState = tmpMouseState;
                MouseUp?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Release, 0));
            };
            //control.PointerWheelChanged += (sender, args) => MouseWheel?.Invoke(new MouseWheelEventArgs(args.))

            control.GotFocus += (sender, args) => FocusedChanged?.Invoke(new FocusedChangedEventArgs(true));
            control.LostFocus += (sender, args) => FocusedChanged?.Invoke(new FocusedChangedEventArgs(false));
            
            _boundsSubscription = control.GetObservable(Visual.BoundsProperty).Subscribe(args =>
            {
                var oldBounds = control.Bounds;
                var newBounds = args;


                if (newBounds.Position != oldBounds.Position)
                {
                    Move?.Invoke(new WindowPositionEventArgs((int) newBounds.Position.X, (int) newBounds.Position.Y));
                }

                if (newBounds.Size != oldBounds.Size)
                {
                    Resize?.Invoke(new ResizeEventArgs((int) newBounds.Size.Width, (int) newBounds.Size.Height));
                }
            });
            if (_window != null)
            {
                _window.Closing += (sender, args) =>
                {
                    var c = new CancelEventArgs(false);
                    Closing?.Invoke(c);
                    args.Cancel = c.Cancel;
                };

                _windowStateSubscription = _window.GetObservable(global::Avalonia.Controls.Window.WindowStateProperty).Subscribe(args =>
                {
                    switch (args)
                    {
                        case global::Avalonia.Controls.WindowState.Minimized:
                            Minimized?.Invoke(new MinimizedEventArgs(true));
                            break;
                        default:
                            Minimized?.Invoke(new MinimizedEventArgs(false));
                            break;
                    }
                });
            }


            _topLevel.Closed += (sender, args) => Closed?.Invoke();
        }

        public void Dispose()
        {
            _boundsSubscription?.Dispose();
            _windowStateSubscription?.Dispose();
        }

        public void Close()
        {
            _window.Close();
        }

        public void ProcessEvents()
        {
            ProcessEvents(-1);
        }

        public bool ProcessEvents(double timeout)
        {
            return true;
        }

        public void MakeCurrent()
        {
            throw new NotImplementedException();
        }

        public Vector2i PointToClient(Vector2i point)
        {
            var res = _control.PointToClient(new PixelPoint(point.X, point.Y));
            return new Vector2i((int) res.X, (int) res.Y);
        }

        public Vector2i PointToScreen(Vector2i point)
        {
            var res = _control.PointToScreen(new global::Avalonia.Point(point.X, point.Y));
            return new Vector2i(res.X, res.Y);
        }

        public bool TryGetCurrentMonitorScale(out float horizontalScale, out float verticalScale)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCurrentMonitorDpi(out float horizontalDpi, out float verticalDpi)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCurrentMonitorDpiRaw(out float horizontalDpi, out float verticalDpi)
        {
            throw new NotImplementedException();
        }

        public bool IsKeyDown(Key key) => KeyboardState.IsKeyDown(key);

        public bool IsKeyUp(Key key) => KeyboardState.IsKeyUp(key);

        public bool IsKeyPressed(Key key) => KeyboardState.IsKeyDown(key) && !LastKeyboardState.IsKeyDown(key);

        public bool IsKeyReleased(Key key) => KeyboardState.IsKeyDown(key) && !LastKeyboardState.IsKeyDown(key);

        /// <inheritdoc />
        public bool IsMouseButtonDown(MouseButton button) => MouseState.IsButtonDown(button);

        /// <inheritdoc />
        public bool IsMouseButtonUp(MouseButton button) => MouseState.IsButtonUp(button);

        /// <inheritdoc />
        public bool IsMouseButtonPressed(MouseButton button) => MouseState.IsButtonDown(button) && !LastMouseState.IsButtonDown(button);

        /// <inheritdoc />
        public bool IsMouseButtonReleased(MouseButton button) => !MouseState.IsButtonDown(button) && LastMouseState.IsButtonDown(button);
        
        public IGraphicsContext Context => throw new NotImplementedException();
        
        public bool IsExiting { get; }
        public string ClipboardString { get; set; }
        public bool Exists => true;
        public WindowIcon Icon { get; set; }
        public bool IsEventDriven { get; set; }
        public Monitor CurrentMonitor { get; set; }
        public ContextAPI API { get; }
        public ContextProfile Profile { get; }
        public ContextFlags Flags { get; }
        public Version APIVersion { get; }
        public string Title { get; set; }
        public bool IsFocused { get; set; }
        public bool IsVisible { get; set; }
        public WindowState WindowState { get; set; }
        public WindowBorder WindowBorder { get; set; }
        public Box2i Bounds { get; set; }
        public Vector2i Location { get; set; }
        public Vector2i Size { get; set; }
        public Box2i ClientRectangle { get; set; }
        public Vector2i ClientSize => new Vector2i((int)_control.Bounds.Width, (int)_control.Bounds.Height);
        public bool IsFullscreen
        {
            get => false;
            set => throw new NotSupportedException();
        }

        public MouseCursor Cursor { get; set; }
        public bool CursorVisible { get; set; }
        public bool CursorGrabbed { get; set; }
        public JoystickState[] JoystickStates => Array.Empty<JoystickState>();
        public JoystickState[] LastJoystickStates => Array.Empty<JoystickState>();

        public KeyboardState KeyboardState { get; private set; }

        public KeyboardState LastKeyboardState{ get; private set; }
        public OpenTK.Mathematics.Vector2 MousePosition { get; set; }
        public OpenTK.Mathematics.Vector2 MouseDelta { get; }

        public MouseState MouseState{ get; private set; }

        public MouseState LastMouseState { get; private set; }

        public bool IsAnyKeyDown => KeyboardState.IsAnyKeyDown;
        public bool IsAnyMouseButtonDown => MouseState.IsAnyButtonDown;


        public event Action<WindowPositionEventArgs> Move;
        public event Action<ResizeEventArgs> Resize;

        public event Action Refresh
        {
            add => throw new NotSupportedException();
            remove => throw new NotSupportedException();
        }

        public event Action<CancelEventArgs> Closing;
        public event Action Closed;
        public event Action<MinimizedEventArgs> Minimized;
        public event Action<JoystickEventArgs> JoystickConnected
        {
            add => throw new NotSupportedException();
            remove => throw new NotSupportedException();
        }
        public event Action<FocusedChangedEventArgs> FocusedChanged;
        public event Action<KeyboardKeyEventArgs> KeyDown;
        public event Action<TextInputEventArgs> TextInput;
        public event Action<KeyboardKeyEventArgs> KeyUp;
        public event Action<MonitorEventArgs> MonitorConnected
        {
            add => throw new NotSupportedException();
            remove => throw new NotSupportedException();
        }
        public event Action MouseLeave;
        public event Action MouseEnter;
        public event Action<MouseButtonEventArgs> MouseDown;
        public event Action<MouseButtonEventArgs> MouseUp;
        public event Action<MouseMoveEventArgs> MouseMove;
        public event Action<MouseWheelEventArgs> MouseWheel;
        public event Action<FileDropEventArgs> FileDrop
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        } // TODO:
    }
}