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

        public static ConfigurationException DuplicateOption(CommandInfo command, string[] options)
        {
            var keys = string.Join(", ", options.Select(x => x.Length > 1 ? $"--{x}" : $"-{x}"));
            if (options.Length > 1)
            {
                return new ConfigurationException($"Options {keys} are duplicated in command '{command.Name}'.");
            }

            return new ConfigurationException($"Option {keys} is duplicated in command '{command.Name}'.");
        }
    }
}
