using System;
using Spectre.Cli;
using Spectre.Console;

namespace Dynamic
{
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
