using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public interface ICommand<TSettings> : ICommandLimiter<TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The validation result.</returns>
        Task<int> Execute(CommandContext context, TSettings settings);
    }
}
