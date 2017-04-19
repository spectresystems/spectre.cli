using System.ComponentModel;
using Spectre.CommandLine;

namespace Example.Commands.Nuget
{
    public abstract class NuGetSettings
    {
        [Option("--non-interactive")]
        [Description("Do not prompt for user input or confirmations.")]
        public bool NonInteractive { get; set; }

        [Option("-s | --source")]
        [Description("Specifies the server URL.")]
        public string Source { get; set; }
    }
}
