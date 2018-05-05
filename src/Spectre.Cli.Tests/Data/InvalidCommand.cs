using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public sealed class InvalidCommand : Command<InvalidSettings>
    {
        public override int Execute(CommandContext context, InvalidSettings settings)
        {
            return 0;
        }
    }
}