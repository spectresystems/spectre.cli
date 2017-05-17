using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Spectre.CommandLine;

namespace Example.Commands.Nuget
{
    public sealed class NuGetPushCommand : Command<NuGetPushCommand.Settings>
    {
        public sealed class Settings : NuGetSettings
        {
            [Required]
            [Argument("[root]", Order = 0)]
            [Description("The Package Id and version.")]
            public string Root { get; set; }

            [Option("-k|--api-key <apiKey>")]
            [Description("The API key for the server.")]
            public string ApiKey { get; set; }

            [Option("-n|--no-symbols")]
            [Description("If a symbols package exists, it will not be pushed to a symbols server.")]
            public bool NoSymbols { get; set; }
        }

        public NuGetPushCommand() 
            : base("push")
        {
        }

        public override int Run(Settings settings)
        {
            return 0;
        }
    }
}