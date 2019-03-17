using System;
using System.Linq;
using Spectre.Cli.Internal.Modelling;
using Spectre.Cli.Internal.Rendering;

namespace Spectre.Cli.Internal.Exceptions
{
    internal class ConfigurationException : CommandAppException
    {
        public override bool AlwaysPropagateWhenDebugging => true;

        public ConfigurationException(string message, IRenderable pretty = null)
            : base(message, pretty)
        {
        }

        public ConfigurationException(string message, Exception ex, IRenderable pretty = null)
            : base(message, ex, pretty)
        {
        }

        public static ConfigurationException NoCommandConfigured()
        {
            return new ConfigurationException("No commands have been configured.");
        }

        public static ConfigurationException CommandNameConflict(CommandInfo command, string alias)
        {
            return new ConfigurationException($"The alias '{alias}' for '{command.Name}' conflicts with another command.");
        }

        public static ConfigurationException DuplicateOption(CommandInfo command, string[] options)
        {
            var keys = string.Join(", ", options.Select(x => x.Length > 1 ? $"--{x}" : $"-{x}"));
            if (options.Length > 1)
            {
                return new ConfigurationException($"Options {keys} are duplicated in command '{command.Name}'.");
            }

            return new ConfigurationException($"Option {keys} is duplicated in command '{command.Name}'.");
        }

        public static ConfigurationException BranchHasNoChildren(CommandInfo command)
        {
            throw new ConfigurationException($"The branch '{command.Name}' does not define any commands.");
        }

        public static ConfigurationException TooManyVectorArguments(CommandInfo command)
        {
            throw new ConfigurationException($"The command '{command.Name}' specifies more than one vector argument.");
        }

        internal static Exception VectorArgumentNotSpecifiedLast(CommandInfo command)
        {
            throw new ConfigurationException($"The command '{command.Name}' specifies an argument vector that is not the last argument.");
        }
    }
}
