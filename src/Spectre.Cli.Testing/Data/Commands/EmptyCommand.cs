namespace Spectre.Cli.Testing.Data.Commands
{
    public sealed class EmptyCommand : Command<EmptyCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
        }

        public override int Execute(CommandContext context, EmptyCommand.Settings settings)
        {
            return 0;
        }
    }
}
