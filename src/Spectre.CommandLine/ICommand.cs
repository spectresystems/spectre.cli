namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command with the specified settings.
        /// </summary>
        /// <param name="settings">The command settings.</param>
        /// <returns>The command's exit code.</returns>
        int Run(object settings);
    }

    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
    {
        /// <summary>
        /// Executes the command with the specified settings.
        /// </summary>
        /// <param name="settings">The command settings.</param>
        /// <returns>The command's exit code.</returns>
        int Run(TSettings settings);
    }
}