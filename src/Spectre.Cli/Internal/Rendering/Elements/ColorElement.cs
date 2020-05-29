using System;

namespace Spectre.Cli.Internal.Rendering
{
    internal sealed class ColorElement : IRenderable
    {
        private readonly ConsoleColor _color;
        private readonly IRenderable _element;

        public int Length => _element.Length;

        public ColorElement(ConsoleColor color, IRenderable element)
        {
            _color = color;
            _element = element;
        }

        public void Render(IRenderer renderer)
        {
            using (renderer.SetForeground(_color))
            {
                _element.Render(renderer);
            }
        }
    }
}
