using System;

namespace Spectre.CommandLine.Internal.Rendering.Elements
{
    internal sealed class ColorElement : IRenderable
    {
        private readonly ConsoleColor _color;
        private readonly IRenderable _element;

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
