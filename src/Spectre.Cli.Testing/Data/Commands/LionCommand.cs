using System.ComponentModel;
using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    [Description("The lion command.")]
    public class LionCommand : AnimalCommand<LionSettings>
    {
        public override int Execute(CommandContext context, LionSettings settings)
        {
            return 0;
        }
    }
}