using Spectre.Cli.Testing.Data.Settings;

namespace Spectre.Cli.Testing.Data.Commands
{
    public sealed class InvalidCommand : Command<InvalidSettings>
    {
        public override int Execute(CommandContext context, InvalidSettings settings)
        {
            return 0;
        }
    }
}