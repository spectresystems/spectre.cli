using System.ComponentModel;
using System.Linq;
using Simple.Commands.Settings;
using Spectre.Cli;

namespace Simple.Commands
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