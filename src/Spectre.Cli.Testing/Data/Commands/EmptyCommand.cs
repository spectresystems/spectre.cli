namespace Spectre.Cli.Testing.Data.Commands
{
    public sealed class EmptyCommand : Command<EmptyCommandSettings>
    {
        public override int Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return 0;
        }
    }

    public sealed class EmptyCommandSettings : CommandSettings
    {
    }
}
