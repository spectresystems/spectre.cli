using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Rendering
{
    internal sealed class ConsoleRenderer : IRenderer
    {
        private readonly Stack<ConsoleColor> _foreground;
        private readonly Stack<ConsoleColor> _background;

        private enum ConsoleColorType
        {
            Foreground,
            Background
        }

        public ConsoleRenderer()
        {
            _foreground = new Stack<ConsoleColor>();
            _background = new Stack<ConsoleColor>();
        }

        private class Scope : IDisposable
        {
            private readonly ConsoleRenderer _renderer;
            private readonly ConsoleColorType _type;

            public Scope(ConsoleRenderer renderer, ConsoleColorType type)
            {
                _renderer = renderer;
                _type = type;
            }

            ~Scope()
            {
                throw new InvalidOperationException("Dispose was never called on console renderer scope.");
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                _renderer.Pop(_type);
            }
        }

        public static void Render(IRenderable renderable)
        {
            var renderer = new ConsoleRenderer();
            renderable.Render(renderer);
        }

        public IDisposable SetBackground(ConsoleColor color)
        {
            Push(ConsoleColorType.Background, color);
            return new Scope(this, ConsoleColorType.Background);
        }

        public IDisposable SetForeground(ConsoleColor color)
        {
            Push(ConsoleColorType.Foreground, color);
            return new Scope(this, ConsoleColorType.Foreground);
        }

        public void Append(string text)
        {
            Console.Write(text);
        }

        private void Push(ConsoleColorType type, ConsoleColor color)
        {
            if (type == ConsoleColorType.Foreground)
            {
                _foreground.Push(Console.ForegroundColor);
                Console.ForegroundColor = color;
            }
            else
            {
                _background.Push(Console.BackgroundColor);
                Console.BackgroundColor = color;
            }
        }

        private void Pop(ConsoleColorType type)
        {
            if (type == ConsoleColorType.Foreground)
            {
                var color = _foreground.Pop();
                Console.ForegroundColor = color;
            }
            else
            {
                var color = _background.Pop();
                Console.BackgroundColor = color;
            }
        }
    }
}
