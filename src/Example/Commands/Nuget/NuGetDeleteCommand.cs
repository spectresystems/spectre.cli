using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Spectre.CommandLine;

namespace Example.Commands.Nuget
{
    public sealed class NuGetDeleteCommand : Command<NuGetDeleteCommand.Settings>
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
        }

        public NuGetDeleteCommand() 
            : base("delete")
        {
        }

        public override int Run(Settings settings)
        {
            return 0;
        }
    }
}