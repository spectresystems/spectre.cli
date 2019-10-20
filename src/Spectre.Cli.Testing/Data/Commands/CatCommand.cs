using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    public class CatCommand : AnimalCommand<CatSettings>
    {
        public override int Execute(CommandContext context, CatSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}
