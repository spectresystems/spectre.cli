using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public class MultipleOptionsCommand : Command<MultipleOptionsSettings>
    {
        public override int Execute(CommandContext context, MultipleOptionsSettings settings)
        {
            return 0;
        }
    }
}