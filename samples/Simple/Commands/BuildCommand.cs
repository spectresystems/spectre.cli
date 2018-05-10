using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Builds a project and all of its dependencies.")]
    public sealed class BuildCommand : Command<BuildSettings>
    {
        public override int Execute(CommandContext context, BuildSettings settings)
        {
            // Return success.
            return 0;
        }
    }
}