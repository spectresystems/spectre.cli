using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Cli.Internal.Exceptions;

namespace Spectre.Cli.Internal.Modelling
{
    internal static class CommandModelValidator
    {
        public static void Validate(CommandModel model)
        {
            if (model.Commands.Count == 0)
            {
                throw ConfigurationException.NoCommandConfigured();
            }

            foreach (var command in model.Commands)
            {
                Validate(command);
            }
        }

        private static void Validate(CommandInfo command)
        {
            // Get duplicate options for command.
            var options = GetDuplicates(command);
            if (options.Length > 0)
            {
                throw ConfigurationException.DuplicateOption(command, options);
            }

            // Validate child commands.
            foreach (var childCommand in command.Children)
            {
                Validate(childCommand);
            }
        }

        private static string[] GetDuplicates(CommandInfo command)
        {
            var result = new Dictionary<string, int>(StringComparer.Ordinal);

            void AddToResult(string key)
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (!result.ContainsKey(key))
                    {
                        result.Add(key, 0);
                    }
                    result[key]++;
                }
            }

            foreach (var option in command.Parameters.OfType<CommandOption>())
            {
                AddToResult(option.ShortName);
                AddToResult(option.LongName);
            }

            return result.Where(x => x.Value > 1)
                .Select(x => x.Key).ToArray();
        }
    }
}
