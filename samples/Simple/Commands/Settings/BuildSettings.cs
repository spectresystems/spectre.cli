using System.ComponentModel;
using Spectre.Cli;

namespace Simple.Commands.Settings
{
    public sealed class BuildSettings : CommandSettings
    {
        [CommandOption("--no-restore")]
        [Description("Doesn't perform an implicit restore during build.")]
        public bool NoRestore { get; set; }
    }
}