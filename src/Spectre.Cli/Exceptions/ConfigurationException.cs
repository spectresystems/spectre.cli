using System;
using System.Linq;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Exceptions
{
    /// <summary>
    /// Represents errors that occur during configuration.
    /// </summary>
    public class ConfigurationException : CommandAppException
    {
        internal override bool AlwaysPropagateWhenDebugging => true;

        internal ConfigurationException(string message, IRenderable? pretty = null)
            : base(message, pretty)
        {
        }

        internal ConfigurationException(string message, Exception ex, IRenderable? pretty = null)
            : base(message, ex, pretty)
        {
        }

        internal static ConfigurationException NoCommandConfigured()
        {
            return new ConfigurationException("No commands have been configured.");
        }

        internal static ConfigurationException CommandNameConflict(CommandInfo command, string alias)
        {
            return new ConfigurationException($"The alias '{alias}' for '{command.Name}' conflicts with another command.");
        }

        internal static ConfigurationException DuplicateOption(CommandInfo command, string[] options)
        {
            var keys = string.Join(", ", options.Select(x => x.Length > 1 ? $"--{x}" : $"-{x}"));
            if (options.Length > 1)
            {
                return new ConfigurationException($"Options {keys} are duplicated in command '{command.Name}'.");
            }

            return new ConfigurationException($"Option {keys} is duplicated in command '{command.Name}'.");
        }

        internal static ConfigurationException BranchHasNoChildren(CommandInfo command)
        {
            throw new ConfigurationException($"The branch '{command.Name}' does not define any commands.");
        }

        internal static ConfigurationException TooManyVectorArguments(CommandInfo command)
        {
            throw new ConfigurationException($"The command '{command.Name}' specifies more than one vector argument.");
        }

        internal static ConfigurationException VectorArgumentNotSpecifiedLast(CommandInfo command)
        {
            throw new ConfigurationException($"The command '{command.Name}' specifies an argument vector that is not the last argument.");
        }

        internal static ConfigurationException OptionalOptionValueMustBeFlagWithValue(CommandOption option)
        {
            return new ConfigurationException($"The option '{option.GetOptionName()}' has an optional value but does not implement IOptionalValue.");
        }
    }
}
