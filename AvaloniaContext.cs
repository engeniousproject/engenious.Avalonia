using System;
using Avalonia.OpenGL;
using OpenTK.Windowing.Common;

namespace engenious.Avalonia
{
    public class AvaloniaContext : IGraphicsContext
    {
        private readonly IGlContext _context;
        private IDisposable? _lastMadeCurrent;

        public AvaloniaContext(IGlContext context)
        {
            _context = context;
        }

        public void SwapBuffers()
        {
            
        }

        public void MakeCurrent()
        {
            _lastMadeCurrent = _context.MakeCurrent();
            IsCurrent = true;
        }

        public void MakeNoneCurrent()
        {
            IsCurrent = false;
            _lastMadeCurrent?.Dispose();
        }

        public bool IsCurrent { get; private set; }
        public IntPtr NativeContex => throw new NotSupportedException();
    }
}