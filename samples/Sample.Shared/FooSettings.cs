using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.Shared
{
    public abstract class FooSettings
    {
        [Option("-f|--foo")]
        [Description("Essential to enable fooing of the bar or baz.")]
        public string Foo { get; set; }
    }
}
