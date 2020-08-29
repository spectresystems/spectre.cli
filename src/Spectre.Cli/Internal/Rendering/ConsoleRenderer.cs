using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Cli.Internal
{
    internal sealed class ConsoleRenderer
    {
        private readonly IAnsiConsole _console;

        public ConsoleRenderer(IConsoleSettings? console)
        {
            _console = (console ?? new ConsoleSettings()).CreateConsole();
        }

        public static void Render(IRenderable? renderable, IConsoleSettings? console)
        {
            var renderer = new ConsoleRenderer(console);
            renderer.Render(renderable);
        }

        public static void Render(IEnumerable<IRenderable?> renderables, IConsoleSettings? console)
        {
            var renderer = new ConsoleRenderer(console);
            foreach (var renderable in renderables)
            {
                renderer.Render(renderable);
            }
        }

        public void Render(IRenderable? renderable)
        {
            if (renderable is null)
            {
                throw new System.ArgumentNullException(nameof(renderable));
            }

            _console.Render(renderable);
        }
    }
}
