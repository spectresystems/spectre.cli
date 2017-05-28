using System;
using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Commands
{
    public sealed class BarCommand : Command<BarCommand.Settings>
    {
        public sealed class Settings : FooSettings
        {
            [Option("-b|--bar")]
            [Description("Triggers a bar.")]
            public string Bar { get; set; }
        }

        public override int Run(Settings settings)
        {
            Console.WriteLine($"Foo={settings.Foo} Bar={settings.Bar}");
            return 0;
        }
    }
}