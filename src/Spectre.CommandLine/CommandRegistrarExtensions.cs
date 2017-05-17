namespace Spectre.CommandLine
{
    public static class CommandRegistrarExtensions
    {
        public static void Register<TCommand>(this ICommandRegistrar registrar)
            where TCommand : ICommand
        {
            registrar.Register(typeof(TCommand));
        }
    }
}
