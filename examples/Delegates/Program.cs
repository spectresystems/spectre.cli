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
                config.AddDelegate("foo", context =>
                {
                    AnsiConsole.MarkupLine("[aqua]Foo[/]");
                    return 0;
                });

                config.AddDelegate<BarSettings>("bar", (context, settings) =>
                {
                    for (var index = 0; index < settings.Count; index++)
                    {
                        AnsiConsole.MarkupLine("[aqua]Bar[/]");
                    }

                    return 0;
                });
            });

            return app.Run(args);
        }
    }
}
