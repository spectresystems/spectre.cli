namespace Spectre.CommandLine
{
    public interface ICommandRegistrar
    {
        void Register<TCommand>() where TCommand : ICommand;
    }
}
