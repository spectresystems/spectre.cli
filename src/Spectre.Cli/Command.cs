using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Base class for a command without settings.
    /// </summary>
    public abstract class Command : ICommand<EmptyCommandSettings>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract int Execute(CommandContext context);

        /// <inheritdoc/>
        Task<int> ICommand<EmptyCommandSettings>.Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return Task.FromResult(Execute(context));
        }

        /// <inheritdoc/>
        Task<int> ICommand.Execute(CommandContext context, CommandSettings settings)
        {
            return Task.FromResult(Execute(context));
        }

        /// <inheritdoc/>
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types")]
        ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
        {
            return ValidationResult.Success();
        }
    }
}
