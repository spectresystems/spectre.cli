using Spectre.Cli.Tests.Data.Settings;

namespace Spectre.Cli.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptySettings>
    {
        public override int Execute(CommandContext context, EmptySettings settings)
        {
            return 0;
        }
    }
}
