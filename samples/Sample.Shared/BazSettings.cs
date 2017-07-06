using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Shared
{
    public abstract class BazSettings : FooSettings
    {
        [Option("-b|--baz")]
        [Description("Re-enables the baz in all sub systems.")]
        public string Baz { get; set; }
    }
}