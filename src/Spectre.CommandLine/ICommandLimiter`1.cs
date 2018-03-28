namespace Spectre.CommandLine
{
    // ReSharper disable once UnusedTypeParameter
    public interface ICommandLimiter<out TSettings> : ICommand
        where TSettings : CommandSettings
    {
    }
}
