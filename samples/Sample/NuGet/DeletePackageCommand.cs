using System.ComponentModel;
using Spectre.CommandLine;
using Spectre.CommandLine.Annotations;

namespace Sample.NuGet
{
    public sealed class DeletePackageCommand : Command<DeletePackageCommand.Settings>
    {
        public sealed class Settings : NuGetSettings
        {
            [Argument(0, "[root]")]
            [Description("The Package Id and version.")]
            public string Root { get; set; }

            [Option("--force-english-output")]
            [Description("Forces the application to run using an invariant, English-based culture.")]
            public bool ForceEnglishOutput { get; set; }

            [Option("-s|--source [source]")] // Required
            [Description("Specifies the server URL.")]
            public string Source { get; set; }

            [Option("--non-interactive")]
            [Description("Do not prompt for user input or confirmations.")]
            public bool NonInteractive { get; set; }

            [Option("-k|--apikey <apikey>")]
            [Description("The API key for the server.")]
            public string ApiKey { get; set; }
        }

        public override int Run(Settings settings)
        {
            return 0;
        }
    }
}
