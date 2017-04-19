using System.ComponentModel;
using Spectre.CommandLine;

namespace Example.Commands.Build
{
    [Description(".NET Builder")]
    public sealed class BuildCommand : Command<BuildCommandSettings>
    {
        public BuildCommand() 
            : base("build")
        {
        }

        public override int Run(BuildCommandSettings commandSettings)
        {
            return 0;
        }
    }
}
