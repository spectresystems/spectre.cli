using System.ComponentModel;
using Spectre.CommandLine;

namespace FakeDotNet.EF
{
    public abstract class EfCommandSettings : EfSettings
    {
        [CommandOption("-c|--context [CONTEXT]")]
        [Description("The DbContext to use.")]
        public string Context { get; set; }

        [CommandOption("-p|--project [PROJECT]")]
        [Description("The project to use.")]
        public string Project { get; set; }

        [CommandOption("-s|--startup-project [PROJECT]")]
        [Description("The startup project to use.")]
        public string StartupProject { get; set; }

        [CommandOption("--framework [FRAMEWORK]")]
        [Description("The target framework.")]
        public string Framework { get; set; }

        [CommandOption("--configuration [CONFIGURATION]")]
        [Description("The configuration to use.")]
        public string Configuration { get; set; }

        [CommandOption("--runtime [IDENTIFIER]")]
        [Description("The runtime to use.")]
        public string Runtime { get; set; }
    }
}
