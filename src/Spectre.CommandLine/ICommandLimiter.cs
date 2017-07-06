namespace Spectre.CommandLine
{
    /// <summary>
    /// Represent a type limiter for commands and should
    /// not be used or implemented directly.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICommandLimiter<out TSettings> : ICommand
    {
    }
}