using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Adds a package reference to a project file.")]
    public sealed class AddPackageCommand : Command<AddPackageSettings>
    {
        public override int Execute(CommandContext context, AddPackageSettings settings)
        {
            // Return success.
            return 0;
        }
    }
}