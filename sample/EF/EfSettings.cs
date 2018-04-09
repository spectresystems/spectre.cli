using System.ComponentModel;
using Spectre.Cli;

namespace FakeDotNet.EF
{
    public abstract class EfSettings : CommandSettings
    {
        [CommandOption("-v|--verbose")]
        [Description("Show verbose output.")]
        public bool Verbose { get; set; }

        [CommandOption("--no-color")]
        [Description("Don't colorize output.")]
        public bool NoColor { get; set; }
    }
}
