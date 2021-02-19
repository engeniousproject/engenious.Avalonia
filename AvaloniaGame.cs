using System;
using System.Reflection;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;

namespace engenious.Avalonia
{
    public class AvaloniaGame : Game<AvaloniaRenderingSurface>
    {
        public event EventHandler<AvaloniaGame>? Loaded;
        /// <inheritdoc />
        public AvaloniaGame(AvaloniaRenderingSurface control)
        {
            control.AttachedToVisualTree += (s,e) =>
            {

            };
            control.Initialized += (sender, args) =>
            {

            };
            control.CreateContext += () =>
            {
                var fieldInfo =
                    typeof(OpenGlControlBase).GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic) ??
                    throw new NotSupportedException("No '_context' field found in Avalonia->OpenGlControlBase");
                if (!(fieldInfo.GetValue(control) is IGlContext val))
                    throw new NotSupportedException("'_context' field in Avalonia->OpenGlControlBase was null instead of a valid IGlContext");
                var context = new AvaloniaContext(val);

                ConstructContext(control, context);
                context.MakeCurrent();
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