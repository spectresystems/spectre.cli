using System.ComponentModel;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
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