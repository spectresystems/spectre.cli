namespace Spectre.CommandLine
{
    public interface ICommand
    {
        int Run(object settings);
    }

    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
    {
        int Run(TSettings settings);
    }
}