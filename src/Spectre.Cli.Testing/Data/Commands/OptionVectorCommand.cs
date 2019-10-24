using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    public class OptionVectorCommand : Command<OptionVectorSettings>
    {
        public override int Execute(CommandContext context, OptionVectorSettings settings)
        {
            return 0;
        }
    }
}