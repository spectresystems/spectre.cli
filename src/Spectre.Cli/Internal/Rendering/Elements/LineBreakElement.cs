using System;

namespace Spectre.Cli.Internal.Rendering
{
    internal sealed class LineBreakElement : IRenderable
    {
        public int Length => 0;

        public void Render(IRenderer renderer)
        {
            renderer.Append(Environment.NewLine);
        }
    }
}