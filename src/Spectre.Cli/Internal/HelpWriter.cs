using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console;
using Spectre.Console.Composition;

namespace Spectre.Cli.Internal
{
    internal static class HelpWriter
    {
        private sealed class HelpArgument
        {
            public string Name { get; }
            public bool Required { get; }
            public string? Description { get; }

            public HelpArgument(string name, bool required, string? description)
            {
                Name = name;
                Required = required;
                Description = description;
            }

            public static IReadOnlyList<HelpArgument> Get(CommandInfo? command)
            {
                var arguments = new List<HelpArgument>();
                arguments.AddRange(command?.Parameters?.OfType<CommandArgument>()?.Select(
                    x => new HelpArgument(x.Value, x.Required, x.Description))
                    ?? Array.Empty<HelpArgument>());
                return arguments;
            }
        }

        private sealed class HelpOption
        {
            public string Short { get; }
            public string Long { get; }
            public string? Value { get; }
            public bool? ValueIsOptional { get; }
            public string? Description { get; }

            public HelpOption(string @short, string @long, string? @value, bool? valueIsOptional, string? description)
            {
                Short = @short;
                Long = @long;
                Value = value;
                ValueIsOptional = valueIsOptional;
                Description = description;
            }

            public static IReadOnlyList<HelpOption> Get(CommandInfo? command)
            {
                var parameters = new List<HelpOption>();
                parameters.Add(new HelpOption("h", "help", null, null, "Prints help information"));
                parameters.AddRange(command?.Parameters?.OfType<CommandOption>()?.Select(o =>
                    new HelpOption(
                        o.ShortNames.FirstOrDefault(), o.LongNames.FirstOrDefault(),
                        o.ValueName, o.ValueIsOptional, o.Description))
                    ?? Array.Empty<HelpOption>());
                return parameters;
            }
        }

        public static IEnumerable<IRenderable> Write(CommandModel model)
        {
            return WriteCommand(model, null);
        }

        public static IEnumerable<IRenderable> WriteCommand(CommandModel model, CommandInfo? command)
        {
            var container = command as ICommandContainer ?? model;
            var isDefaultCommand = command?.IsDefaultCommand ?? false;

            var result = new List<IRenderable>();
            result.AddRange(GetUsage(model, command));
            result.AddRange(GetExamples(model, command));
            result.AddRange(GetArguments(command));
            result.AddRange(GetOptions(command));
            result.AddRange(GetCommands(model, container, isDefaultCommand));

            return result;
        }

        private static IEnumerable<IRenderable> GetUsage(CommandModel model, CommandInfo? command)
        {
            var composer = new Composer();
            composer.Style("yellow", "USAGE:").LineBreak();
            composer.Tab().Text(model.GetApplicationName());

            var parameters = new Stack<string>();

            if (command == null)
            {
                parameters.Push("[aqua]<COMMAND>[/]");
                parameters.Push("[grey][[OPTIONS][/]");
            }
            else
            {
                var current = command;
                if (command.IsBranch)
                {
                    parameters.Push("[aqua]<COMMAND>[/]");
                }

                while (current != null)
                {
                    var isCurrent = current == command;
                    if (isCurrent)
                    {
                        parameters.Push("[grey][[OPTIONS][/]");
                    }

                    if (current.Parameters.OfType<CommandArgument>().Any())
                    {
                        var optionalArguments = current.Parameters.OfType<CommandArgument>().Where(x => !x.Required).ToArray();
                        if (optionalArguments.Length > 0 || !isCurrent)
                        {
                            foreach (var optionalArgument in optionalArguments)
                            {
                                parameters.Push($"[grey][[{optionalArgument.Value.SafeMarkup()}][/]");
                            }
                        }

                        if (isCurrent)
                        {
                            foreach (var argument in current.Parameters.OfType<CommandArgument>()
                                .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                            {
                                parameters.Push($"[aqua]<{argument.Value.SafeMarkup()}>[/]");
                            }
                        }
                    }

                    if (!current.IsDefaultCommand)
                    {
                        if (isCurrent)
                        {
                            parameters.Push($"[underline]{current.Name.SafeMarkup()}[/]");
                        }
                        else
                        {
                            parameters.Push($"{current.Name.SafeMarkup()}");
                        }
                    }

                    current = current.Parent;
                }
            }

            composer.Join(" ", parameters);
            composer.LineBreaks(2);

            return new[]
            {
                composer,
            };
        }

        private static IEnumerable<IRenderable> GetExamples(CommandModel model, CommandInfo? command)
        {
            var maxExamples = int.MaxValue;

            var examples = command?.Examples ?? model.Examples ?? new List<string[]>();
            if (examples.Count == 0)
            {
                // Since we're not checking direct examples,
                // make sure that we limit the number of examples.
                maxExamples = 5;

                // Get the current root command.
                var root = command ?? (ICommandContainer)model;
                var queue = new Queue<ICommandContainer>(new[] { root });

                // Traverse the command tree and look for examples.
                // As soon as a node contains commands, bail.
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    foreach (var cmd in current.Commands)
                    {
                        if (cmd.Examples.Count > 0)
                        {
                            examples.AddRange(cmd.Examples);
                        }

                        queue.Enqueue(cmd);
                    }

                    if (examples.Count >= maxExamples)
                    {
                        break;
                    }
                }
            }

            if (examples.Count > 0)
            {
                var composer = new Composer();
                composer.Style("yellow", "EXAMPLES:").LineBreak();

                for (var index = 0; index < Math.Min(maxExamples, examples.Count); index++)
                {
                    var args = string.Join(" ", examples[index]);
                    composer.Tab().Text(model.GetApplicationName()).Space().Style("grey", args);
                    composer.LineBreak();
                }

                composer.LineBreak();
                return new[] { composer };
            }

            return Array.Empty<IRenderable>();
        }

        private static IEnumerable<IRenderable> GetArguments(CommandInfo? command)
        {
            var arguments = HelpArgument.Get(command);
            if (arguments.Count == 0)
            {
                return Array.Empty<IRenderable>();
            }

            var result = new List<IRenderable>();

            result.Add(Text.New("[yellow]ARGUMENTS:[/]"));
            result.Add(Text.New("\n"));

            var grid = new Grid();
            grid.AddColumn(new GridColumn { Padding = new Padding(4, 3), NoWrap = true });
            grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

            foreach (var argument in arguments)
            {
                if (argument.Required)
                {
                    grid.AddRow(
                        $"[aqua]<{argument.Name.SafeMarkup()}>[/]",
                        argument.Description?.TrimEnd('.') ?? string.Empty);
                }
                else
                {
                    grid.AddRow(
                        $"[aqua][[{argument.Name.SafeMarkup()}][/]",
                        argument.Description?.TrimEnd('.') ?? string.Empty);
                }
            }

            grid.AddRow(string.Empty, string.Empty);
            result.Add(grid);

            return result;
        }

        private static IEnumerable<IRenderable> GetOptions(CommandInfo? command)
        {
            // Collect all options into a single structure.
            var parameters = HelpOption.Get(command);
            if (parameters.Count == 0)
            {
                return Array.Empty<IRenderable>();
            }

            var result = new List<IRenderable>();
            result.Add(Text.New("[yellow]OPTIONS:[/]"));
            result.Add(Text.New("\n"));

            var grid = new Grid();
            grid.AddColumn(new GridColumn { Padding = new Padding(4, 3), NoWrap = true });
            grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

            foreach (var option in parameters.ToArray())
            {
                var parts = new StringBuilder();
                if (option.Short != null)
                {
                    parts.Append("[aqua]-").Append(option.Short.SafeMarkup()).Append("[/]");
                    if (option.Long != null)
                    {
                        parts.Append(", ");
                    }
                }

                if (option.Long != null)
                {
                    parts.Append("[aqua]--").Append(option.Long.SafeMarkup()).Append("[/]");
                }

                if (option.Value != null)
                {
                    parts.Append(" ");
                    if (option.ValueIsOptional ?? false)
                    {
                        parts.Append("[grey][[").Append(option.Value.SafeMarkup()).Append("][/]");
                    }
                    else
                    {
                        parts.Append("[grey]<").Append(option.Value.SafeMarkup()).Append(">[/]");
                    }
                }

                grid.AddRow(
                    parts.ToString(),
                    option.Description?.TrimEnd('.') ?? string.Empty);
            }

            grid.AddRow(string.Empty, string.Empty);
            result.Add(grid);

            return result;
        }

        private static IEnumerable<IRenderable> GetCommands(
            CommandModel model,
            ICommandContainer command,
            bool isDefaultCommand)
        {
            var commands = isDefaultCommand ? model.Commands : command.Commands;
            commands = commands.Where(x => !x.IsHidden).ToList();

            if (commands.Count == 0)
            {
                return Array.Empty<IRenderable>();
            }

            var result = new List<IRenderable>();
            result.Add(Text.New("[yellow]COMMANDS:[/]"));
            result.Add(Text.New("\n"));

            var grid = new Grid();
            grid.AddColumn(new GridColumn { Padding = new Padding(4, 3), NoWrap = true });
            grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

            foreach (var child in commands)
            {
                grid.AddRow(
                    $"[aqua]{child.Name.SafeMarkup()}[/]",
                    child.Description?.TrimEnd('.') ?? string.Empty);
            }

            grid.AddRow(string.Empty, string.Empty);
            result.Add(grid);

            return result;
        }
    }
}
