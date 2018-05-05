using System.ComponentModel;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Adds project-to-project (P2P) references.")]
    public sealed class AddReferenceCommand : Command<AddReferenceSettings>
    {
        public override int Execute(CommandContext context, AddReferenceSettings settings)
        {
            // Return success.
            return 0;
        }
    }
}