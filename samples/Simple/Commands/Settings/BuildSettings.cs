using System.Collections.Generic;
using System.ComponentModel;
using Spectre.Cli;

namespace Sample.Commands.Settings
{
    public sealed class BuildSettings : CommandSettings
    {
        [CommandOption("--no-restore")]
        [Description("Doesn't perform an implicit restore during build.")]
        public bool NoRestore { get; set; }

        [CommandOption("--env <KEY=VALUE>")]
        [Description("One or many environment variables")]
        public IDictionary<string, string> EnvironmentVariables { get; set; }
    }
}