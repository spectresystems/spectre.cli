using System;
using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.CommandLine;

namespace Sample.Commands
{
    [Description("The bar command.")]
    public sealed class BarCommand : Command<BarSettings>
    {
        public override int Run(BarSettings settings)
        {
            Console.WriteLine($"Foo={settings.Foo} Bar={settings.Bar}");
            return 0;
        }
    }
}