using System;
using System.Reflection;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;

namespace engenious.Avalonia
{
    public class AvaloniaGame : Game<AvaloniaRenderingSurface>
    {
        public event EventHandler<AvaloniaGame> Loaded;
        /// <inheritdoc />
        public AvaloniaGame(AvaloniaRenderingSurface control)
        {
            control.CreateContext += () =>
            {
                var fieldInfo =
                    typeof(OpenGlControlBase).GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic);
                var context = new AvaloniaContext(
                    (IGlContext)fieldInfo.GetValue(control));

                ConstructContext(control, context);

                InitializeControl();
            };
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Loaded?.Invoke(this, this);
        }
    }
}