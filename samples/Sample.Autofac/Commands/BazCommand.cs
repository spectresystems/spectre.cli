using System;
using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Autofac.Commands
{
    public sealed class BazCommand : Command<BazCommand.Settings>
    {
        public sealed class Settings : FooSettings
        {
            [Option("-b|--baz")]
            [Description("Re-enables the baz in all sub systems.")]
            public string Baz { get; set; }

            public Settings(Greeting greeting) 
                : base(greeting)
            {
            }
        }

        public override int Run(Settings settings)
        {
            Console.WriteLine(settings.Greeting.GetGreetingPhrase());
            Console.WriteLine($"Foo={settings.Foo} Baz={settings.Baz}");
            return 0;
        }
    }
}