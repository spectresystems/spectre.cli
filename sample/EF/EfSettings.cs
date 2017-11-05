using System.ComponentModel;
using Spectre.CommandLine;

namespace Sample.EF
{
    public abstract class EfSettings
    {
        [CommandOption("-v|--verbose")]
        [Description("Show verbose output.")]
        public bool Verbose { get; set; }

        [CommandOption("--no-color")]
        [Description("Don't colorize output.")]
        public bool NoColor { get; set; }
    }
}
