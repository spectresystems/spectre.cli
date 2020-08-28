using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Cli.Internal
{
    internal sealed class ConsoleRenderer
    {
        private readonly IAnsiConsole _console;

        public ConsoleRenderer(IAnsiConsole? console)
        {
            _console = console ?? AnsiConsole.Console;
        }

        public static void Render(IRenderable? renderable, IAnsiConsole? console)
        {
            var renderer = new ConsoleRenderer(console);
            renderer.Render(renderable);
        }

        public static void Render(IEnumerable<IRenderable?> renderables, IAnsiConsole? console)
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
