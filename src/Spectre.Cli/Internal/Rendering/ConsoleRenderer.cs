using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Cli.Internal
{
    internal sealed class ConsoleRenderer : IRenderer
    {
        private readonly Stack<ConsoleColor> _foreground;
        private readonly Stack<ConsoleColor> _background;
        private readonly IConsoleWriter _console;

        private enum ConsoleColorType
        {
            Foreground,
            Background,
        }

        public ConsoleRenderer(IConsoleWriter? console)
        {
            _foreground = new Stack<ConsoleColor>();
            _background = new Stack<ConsoleColor>();
            _console = console ?? new DefaultConsoleWriter();
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

            [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Trust me")]
            [SuppressMessage("Performance", "CA1821:Remove empty Finalizers", Justification = "Trust me")]
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

        public static void Render(IRenderable renderable, IConsoleWriter? console)
        {
            var renderer = new ConsoleRenderer(console);
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
            _console.Write(text);
        }

        private void Push(ConsoleColorType type, ConsoleColor color)
        {
            if (type == ConsoleColorType.Foreground)
            {
                _foreground.Push(Console.ForegroundColor);
                _console.ForegroundColor = color;
            }
            else
            {
                _background.Push(Console.BackgroundColor);
                _console.BackgroundColor = color;
            }
        }

        private void Pop(ConsoleColorType type)
        {
            if (type == ConsoleColorType.Foreground)
            {
                var color = _foreground.Pop();
                _console.ForegroundColor = color;
            }
            else
            {
                var color = _background.Pop();
                _console.BackgroundColor = color;
            }
        }
    }
}
