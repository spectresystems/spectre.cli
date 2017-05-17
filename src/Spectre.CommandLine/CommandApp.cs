namespace Spectre.CommandLine
{
    public sealed class CommandApp : CommandAppBase<CommandAppSettings>
    {
        public CommandApp() 
            : base(new CommandAppSettings())
        {
        }

        protected override void Initialize()
        {
        }

        public void RegisterCommand<TCommand>()
            where TCommand : ICommand
        {
            RegisterCommand(typeof(TCommand));
        }
    }
}