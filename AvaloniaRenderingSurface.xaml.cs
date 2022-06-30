using System;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using TextInputEventArgs = OpenTK.Windowing.Common.TextInputEventArgs;

namespace engenious.Avalonia
{
    public class AvaloniaRenderingSurface : OpenGlControlBase, IRenderingSurface
    {
        class AvaloniaBindingContext : IBindingsContext
        {
            private readonly GlInterface _glInterface;
            public AvaloniaBindingContext(GlInterface glInterface)
            {
                _glInterface = glInterface;
            }

            public IntPtr GetProcAddress(string procName) => _glInterface.GetProcAddress(procName);
        }

        private AvaloniaControlWrapper CreateWrapper()
        {
            var wrapper = new AvaloniaControlWrapper(this);
            wrapper.Closing += Closing;
            return wrapper;
        }

        public AvaloniaRenderingSurface()
        {
            if (Parent != null)
            {
                WindowInfo = CreateWrapper();
            }
            AttachedToVisualTree += (sender, args) =>
            {
                WindowInfo?.Dispose();
                WindowInfo = CreateWrapper();
            };
            DetachedFromVisualTree += (sender, args) =>
            {
                WindowInfo?.Dispose();
                WindowInfo = null;
            };

            _boundsSubscription = this.GetObservable(BoundsProperty).Subscribe(args =>
            {
                var oldBounds = this.Bounds;
                var newBounds = args;
                Resize?.Invoke(new ResizeEventArgs((int) newBounds.Size.Width, (int) newBounds.Size.Height));
            });
            
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
        }
        protected override void OnOpenGlInit(GlInterface gl, int fb)
        {
            base.OnOpenGlInit(gl, fb);
            
            var tmp = new AvaloniaBindingContext(gl);
            OpenTK.Graphics.ES11.GL.LoadBindings(tmp);
            OpenTK.Graphics.ES20.GL.LoadBindings(tmp);
            OpenTK.Graphics.ES30.GL.LoadBindings(tmp);
            OpenTK.Graphics.OpenGL.GL.LoadBindings(tmp);
            OpenTK.Graphics.OpenGL4.GL.LoadBindings(tmp);

            InitializeContext(false);
        }

        private bool _isInitialized;

        public void InitializeContext(bool reinitialize = true)
        {
            if (reinitialize && !_isInitialized)
                return;
            _isInitialized = true;
            CreateContext?.Invoke();
            
            Load?.Invoke();
            _stopwatch.Start();
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            var elapsed = _stopwatch.ElapsedMilliseconds / 1000.0;

            if (elapsed <= 0)
            {
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                    Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
                }
                return;
            }
            _stopwatch.Restart();
            try
            {
                UpdateFrame?.Invoke(new FrameEventArgs(elapsed));
                RenderFrame?.Invoke(new FrameEventArgs(elapsed));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }
        
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            MouseWheel?.Invoke(new MouseWheelEventArgs((float)e.Delta.X, (float)e.Delta.Y));
        }

        protected override void OnTextInput(global::Avalonia.Input.TextInputEventArgs e)
        {
            base.OnTextInput(e);
            if (e.Text == null)
                return;
            foreach (var c in e.Text)
            {
                KeyPress?.Invoke(new TextInputEventArgs(c));
            }
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            FocusedChanged?.Invoke(new FocusedChangedEventArgs(true));
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            FocusedChanged?.Invoke(new FocusedChangedEventArgs(false));
        }

        public void Dispose()
        {
            WindowInfo?.Dispose();
        }

        public Point PointToClient(Point point)
        {
            var res = this.PointToClient(new PixelPoint(point.X, point.Y));
            return new Point((int) res.X, (int) res.Y);
        }
        
        /// <inheritdoc />
        public Vector2 Vector2ToScreen(Vector2 pt)
        {
            var integerPart = new Point((int) pt.X, (int) pt.Y);
            var tmp = PointToScreen(integerPart);
            return  tmp.ToVector2() + pt - integerPart.ToVector2();
        }

        /// <inheritdoc />
        public Vector2 Vector2ToClient(Vector2 pt)
        {
            var integerPart = new Point((int) pt.X, (int) pt.Y);
            var tmp = PointToClient(integerPart);
            return  tmp.ToVector2() + pt - integerPart.ToVector2();
        }

        public Point PointToScreen(Point point)
        {
            var res = this.PointToScreen(new global::Avalonia.Point(point.X, point.Y));
            return new Point(res.X, res.Y);
        }

        public Rectangle ClientRectangle
        {
            get => new Rectangle((int)0, (int)0, (int)Bounds.Width, (int)Bounds.Height);
            set => Bounds = new Rect(Bounds.X, Bounds.Y, value.Width, value.Height);
        }

        public Size ClientSize
        {
            get => new Size((int)Bounds.Width, (int)Bounds.Height);
            set => Bounds = new Rect(Bounds.X, Bounds.Y, value.Width, value.Height);
        }

        public bool Focused => IsFocused;

        private readonly Cursor _invisibleCursor = new Cursor(StandardCursorType.None);
        private IDisposable _boundsSubscription;

        public bool CursorVisible
        {
            get => Cursor != _invisibleCursor;
            set => Cursor = value ? Cursor.Default : _invisibleCursor;
        }

        public bool CursorGrabbed { get; set; }

        public bool Visible
        {
            get => IsVisible;
            set => IsVisible = value;
        }

        public IntPtr Handle => throw new NotSupportedException();
        public IWindowWrapper? WindowInfo { get; private set; }
        
        public event Action<FrameEventArgs>? RenderFrame;
        public event Action<FrameEventArgs>? UpdateFrame;
        public event Action<CancelEventArgs>? Closing;
        public event Action<FocusedChangedEventArgs>? FocusedChanged;
        public event Action<TextInputEventArgs>? KeyPress;
        public event Action<ResizeEventArgs>? Resize;
        public event Action? Load;
        public event Action<MouseWheelEventArgs>? MouseWheel;

        public event Action? CreateContext;
    }
}