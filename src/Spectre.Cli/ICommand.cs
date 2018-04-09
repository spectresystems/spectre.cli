using System.Linq;
using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Validates the specified settings and remaining arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="remaining">The remaining arguments.</param>
        /// <returns>The validation result.</returns>
        ValidationResult Validate(object settings, ILookup<string, string> remaining);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="remaining">The remaining arguments.</param>
        /// <returns>The validation result.</returns>
        Task<int> Execute(object settings, ILookup<string, string> remaining);
    }
}