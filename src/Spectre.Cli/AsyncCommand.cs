using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Base class for an asynchronous command with no settings.
    /// </summary>
    public abstract class AsyncCommand : ICommand<EmptyCommandSettings>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract Task<int> ExecuteAsync(CommandContext context);

        Task<int> ICommand<EmptyCommandSettings>.Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return ExecuteAsync(context);
        }

        Task<int> ICommand.Execute(CommandContext context, CommandSettings settings)
        {
            return ExecuteAsync(context);
        }

        ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
        {
            return ValidationResult.Success();
        }
    }
}