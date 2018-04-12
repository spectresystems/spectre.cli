using System.ComponentModel;
using System.Linq;
using Simple.Commands.Settings;
using Spectre.Cli;

namespace Simple.Commands
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