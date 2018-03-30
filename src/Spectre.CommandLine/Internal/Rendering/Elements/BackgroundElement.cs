using System;

namespace Spectre.CommandLine.Internal.Rendering.Elements
{
    internal sealed class BackgroundElement : IRenderable
    {
        private readonly ConsoleColor _color;
        private readonly IRenderable _element;

        public BackgroundElement(ConsoleColor color, IRenderable element)
        {
            _color = color;
            _element = element;
        }

        public void Render(IRenderer renderer)
        {
            using (renderer.SetBackground(_color))
            {
                _element.Render(renderer);
            }
        }
    }
}