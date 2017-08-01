using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Tests.Data
{
    public class FooSettings
    {
        [Option("-f|--foo")]
        public string Foo { get; set; }
    }
}