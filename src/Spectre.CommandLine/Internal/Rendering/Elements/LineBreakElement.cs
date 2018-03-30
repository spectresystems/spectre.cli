using System;

namespace Spectre.CommandLine.Internal.Rendering.Elements
{
    internal sealed class LineBreakElement : IRenderable
    {
        public void Render(IRenderer renderer)
        {
            renderer.Append(Environment.NewLine);
        }
    }
}