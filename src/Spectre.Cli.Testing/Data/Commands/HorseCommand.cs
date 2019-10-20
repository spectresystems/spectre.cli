using System.ComponentModel;
using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
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
