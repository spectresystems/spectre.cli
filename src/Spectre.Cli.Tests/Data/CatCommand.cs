using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
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
