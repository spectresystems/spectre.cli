using System.Diagnostics;
using System.Threading.Tasks;

namespace Spectre.Cli
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
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The validation result.</returns>
        public virtual ValidationResult Validate(CommandContext context, TSettings settings)
        {
            return ValidationResult.Success();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The validation result.</returns>
        public abstract Task<int> Execute(CommandContext context, TSettings settings);

        ValidationResult ICommand.Validate(CommandContext context, object settings)
        {
            return Validate(context, (TSettings)settings);
        }

        Task<int> ICommand.Execute(CommandContext context, object settings)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Execute(context, (TSettings)settings);
        }
    }
}