using System.ComponentModel;
using System.Linq;
using Sample.Commands.Settings;
using Spectre.Cli;

namespace Sample.Commands
{
    [Description("Adds project-to-project (P2P) references.")]
    public sealed class AddReferenceCommand : Command<AddReferenceSettings>
    {
        public override int Execute(AddReferenceSettings settings, ILookup<string, string> remaining)
        {
            // Return success.
            return 0;
        }
    }
}