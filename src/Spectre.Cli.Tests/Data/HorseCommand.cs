using System.ComponentModel;
using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    [Description("The horse command.")]
    public class HorseCommand : AnimalCommand<MammalSettings>
    {
        public override int Execute(CommandContext context, MammalSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}
