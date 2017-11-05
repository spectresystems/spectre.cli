namespace Spectre.CommandLine
{
    public interface ICommandLimiter<out TSettings> : ICommand
        where TSettings : class
    {
    }
}
