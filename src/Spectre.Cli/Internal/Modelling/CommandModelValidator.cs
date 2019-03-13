using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Cli.Internal.Exceptions;
using Spectre.Cli.Internal.Parsing;

namespace Spectre.Cli.Internal.Modelling
{
    internal static class CommandModelValidator
    {
        public static void Validate(CommandModel model, ConfigurationSettings settings)
        {
            if (model.Commands.Count == 0 && model.DefaultCommand == null)
            {
                throw ConfigurationException.NoCommandConfigured();
            }

            foreach (var command in model.Commands)
            {
                foreach (var alias in command.Aliases)
                {
                    if (model.Commands.Any(x => x.Name.Equals(alias, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw ConfigurationException.CommandNameConflict(command, alias);
                    }
                }
            }

            foreach (var command in model.Commands)
            {
                Validate(command);
            }

            if (settings.ValidateExamples)
            {
                ValidateExamples(model);
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

            // No children?
            if (command.IsBranch && command.Children.Count == 0)
            {
                throw ConfigurationException.BranchHasNoChildren(command);
            }

            // Validate child commands.
            foreach (var childCommand in command.Children)
            {
                Validate(childCommand);
            }
        }

        private static void ValidateExamples(CommandModel model)
        {
            var examples = new List<string[]>();
            examples.AddRangeIfNotNull(model.Examples);

            // Get all examples.
            var queue = new Queue<ICommandContainer>(new[] { model });
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var command in current.Commands)
                {
                    examples.AddRangeIfNotNull(command.Examples);
                    queue.Enqueue(command);
                }
            }

            // Validate all examples.
            foreach (var example in examples)
            {
                try
                {
                    var parser = new CommandTreeParser(model);
                    parser.Parse(example);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("Validation of examples failed.", ex);
                }
            }
        }

        private static string[] GetDuplicates(CommandInfo command)
        {
            var result = new Dictionary<string, int>(StringComparer.Ordinal);

            void AddToResult(IEnumerable<string> keys)
            {
                foreach (var key in keys)
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
            }

            foreach (var option in command.Parameters.OfType<CommandOption>())
            {
                AddToResult(option.ShortNames);
                AddToResult(option.LongNames);
            }

            return result.Where(x => x.Value > 1)
                .Select(x => x.Key).ToArray();
        }
    }
}
