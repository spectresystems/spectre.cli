using System;
using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.CommandLine;

namespace Sample.Commands
{
    [Description("The qux command.")]
    public sealed class QuxCommand : Command<QuxSettings>
    {
        public override int Run(QuxSettings settings)
        {
            Console.WriteLine($"Foo={settings.Foo} Baz={settings.Baz} Qux={settings.Qux}");
            return 0;
        }
    }
}
