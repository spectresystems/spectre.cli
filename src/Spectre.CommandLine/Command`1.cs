using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Base class for a command.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class Command<TSettings> : ICommand<TSettings>
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
        public abstract int Execute(TSettings settings, ILookup<string, string> remaining);

        ValidationResult ICommand.Validate(object settings, ILookup<string, string> remaining)
        {
            return Validate((TSettings)settings, remaining);
        }

        Task<int> ICommand.Execute(object settings, ILookup<string, string> remaining)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Task.FromResult(Execute((TSettings)settings, remaining));
        }

        Task<int> ICommand<TSettings>.Execute(TSettings settings, ILookup<string, string> remaining)
        {
            return Task.FromResult(Execute(settings, remaining));
        }
    }
}
