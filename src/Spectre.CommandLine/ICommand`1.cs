using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
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
        /// <param name="settings">The settings.</param>
        /// <param name="remaining">The remaining arguments.</param>
        /// <returns>The validation result.</returns>
        Task<int> Execute(TSettings settings, ILookup<string, string> remaining);
    }
}
