using System.ComponentModel;
using System.Linq;
using Simple.Commands.Settings;
using Spectre.Cli;

namespace Simple.Commands
{
    [Description("Adds a package reference to a project file.")]
    public sealed class AddPackageCommand : Command<AddPackageSettings>
    {
        public override int Execute(AddPackageSettings settings, ILookup<string, string> remaining)
        {
            // Return success.
            return 0;
        }
    }
}