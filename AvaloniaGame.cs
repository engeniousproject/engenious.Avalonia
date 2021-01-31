using System.Reflection;
using Avalonia.OpenGL;

namespace engenious.Avalonia
{
    public class AvaloniaGame : Game<AvaloniaRenderingSurface>
    {
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
    }
}