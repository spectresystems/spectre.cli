using Spectre.Cli;
using Spectre.Console;

namespace Delegates
{
    public static partial class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddDelegate("foo", Foo)
                    .WithDescription("Foos the bars");

                config.AddDelegate<BarSettings>("bar", Bar)
                    .WithDescription("Bars the foos"); ;
            });

            return app.Run(args);
        }

        private static int Foo(CommandContext context)
        {
            AnsiConsole.MarkupLine("[aqua]Foo[/]");
            return 0;
        }

        private static int Bar(CommandContext context, BarSettings settings)
        {
            for (var index = 0; index < settings.Count; index++)
            {
                AnsiConsole.MarkupLine("[aqua]Bar[/]");
            }

            return 0;
        }
    }
}
