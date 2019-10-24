using System.ComponentModel;
using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    [Description("The giraffe command.")]
    public sealed class GiraffeCommand : Command<GiraffeSettings>
    {
        public override int Execute(CommandContext context, GiraffeSettings settings)
        {
            return 0;
        }
    }
}
