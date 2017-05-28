using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Autofac.Commands
{
    public abstract class FooSettings
    {
        public Greeting Greeting { get; }

        [Option("-f|--foo")]
        [Description("Essential to enable fooing of the bar or baz.")]
        public string Foo { get; set; }

        protected FooSettings(Greeting greeting)
        {
            Greeting = greeting;
        }
    }
}
