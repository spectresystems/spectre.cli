using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Sample.Commands.Settings
{
    public abstract class BazSettings : FooSettings
    {
        [Option("-b|--baz <BAZ_VALUE>")]
        [Description("Re-enables the baz in all sub systems.")]
        public string Baz { get; set; }
    }
}