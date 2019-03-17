using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public class OptionVectorCommand : Command<OptionVectorSettings>
    {
        public override int Execute(CommandContext context, OptionVectorSettings settings)
        {
            return 0;
        }
    }
}