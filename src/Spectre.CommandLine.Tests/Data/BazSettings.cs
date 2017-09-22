using System.ComponentModel;
using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Tests.Data
{
    public sealed class BazSettings : FooSettings
    {
        [Option("-b|--baz")]
        [DefaultValue(3)]
        public int Baz { get; set; }

        [Argument(0, "KEX_VALUE")]
        public string Alpha { get; set; }

        [Argument(1, "BRAVO_VALUE")]
        public string Beta { get; set; }
    }
}
