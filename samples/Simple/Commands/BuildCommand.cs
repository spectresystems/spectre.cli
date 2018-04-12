using System.ComponentModel;
using System.Linq;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Builds a project and all of its dependencies.")]
    public sealed class BuildCommand : Command<BuildSettings>
    {
        public override int Execute(BuildSettings settings, ILookup<string, string> remaining)
        {
            // Return success.
            return 0;
        }
    }
}