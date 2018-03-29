using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Base class for a command that work with async/await.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class AsyncCommand<TSettings> : ICommand<TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Validates the specified settings and remaining arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="remaining">The remaining arguments.</param>
        /// <returns>The validation result.</returns>
        public virtual ValidationResult Validate(TSettings settings, ILookup<string, string> remaining)
        {
            return ValidationResult.Success();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="remaining">The remaining arguments.</param>
        /// <returns>The validation result.</returns>
        public abstract Task<int> Execute(TSettings settings, ILookup<string, string> remaining);

        ValidationResult ICommand.Validate(object settings, ILookup<string, string> remaining)
        {
            return Validate((TSettings)settings, remaining);
        }

        Task<int> ICommand.Execute(object settings, ILookup<string, string> remaining)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Execute((TSettings)settings, remaining);
        }
    }
}