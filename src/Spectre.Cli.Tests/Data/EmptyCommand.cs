using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptyCommandSettings>
    {
        public override int Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return 0;
        }
    }
}
