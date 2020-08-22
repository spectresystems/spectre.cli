using System;
using System.Linq;
using Spectre.Cli;
using Spectre.Console;

namespace Dynamic
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                foreach(var index in Enumerable.Range(1, 10))
                {
                    config.AddCommand<MyCommand>($"c{index}")
                        .WithDescription($"Prints the number {index}")
                        .WithData(index);
                }
            });

            return app.Run(args);
        }
    }

    public sealed class MyCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            if (!(context.Data is int data))
            {
                throw new InvalidOperationException("Command has no associated data.");
                
            }

            AnsiConsole.MarkupLine("Value = [aqua]{0}[/]", data);
            return 0;
        }
    }
}
