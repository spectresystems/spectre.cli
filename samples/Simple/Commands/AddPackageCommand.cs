using System.ComponentModel;
using System.Linq;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
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