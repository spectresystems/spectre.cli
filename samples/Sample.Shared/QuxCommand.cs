using System;
using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Shared
{
    public sealed class QuxCommand : Command<QuxCommand.Settings>
    {
        public sealed class Settings : BazSettings
        {
            [Option("-q|--qux")]
            [Description("Sets the qux timestamp for the current baz.")]
            public DateTime Qux { get; set; }
        }

        public override int Run(Settings settings)
        {
            Console.WriteLine($"Foo={settings.Foo} Baz={settings.Baz} Qux={settings.Qux}");
            return 0;
        }
    }
}
