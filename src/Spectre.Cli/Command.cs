using System.Threading.Tasks;

namespace Spectre.Cli
{
    /// <summary>
    /// Base class for a command without settings.
    /// </summary>
    public abstract class Command : ICommand<EmptyCommandSettings>
    {
        /// <summary>
        /// The settings for the <see cref="Command"/> instance.
        /// </summary>
        public sealed class Settings : CommandSettings
        {
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract int Execute(CommandContext context);

        Task<int> ICommand<EmptyCommandSettings>.Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return Task.FromResult(Execute(context));
        }

        Task<int> ICommand.Execute(CommandContext context, CommandSettings settings)
        {
            return Task.FromResult(Execute(context));
        }

        ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
        {
            return ValidationResult.Success();
        }
    }
}
