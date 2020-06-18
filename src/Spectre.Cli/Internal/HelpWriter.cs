using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        }

        public static IRenderable Write(CommandModel model)
        {
            return WriteCommand(model, null);
        }

        public static IRenderable WriteCommand(CommandModel model, CommandInfo? command)
        {
            var composer = new RenderableComposer();
            composer.LineBreak();

            // Usage
            WriteUsage(composer, model, command);
            composer.LineBreak().LineBreak();

            // Examples
            WriteExamples(composer, model, command);

            // Arguments
            WriteArguments(composer, command);

            // Options
            WriteOptions(composer, command);

            // Commands
            WriteCommands(
                composer, model,
                command as ICommandContainer ?? model,
                command?.IsDefaultCommand ?? false);

            composer.LineBreak();
            return composer;
        }

        private static void WriteUsage(RenderableComposer composer, CommandModel model, CommandInfo? command)
        {
            var parameters = new Stack<IRenderable>();

            if (command == null)
            {
                parameters.Push(new ColorElement(ConsoleColor.Cyan, new TextElement("<COMMAND>")));
                parameters.Push(new ColorElement(ConsoleColor.DarkGray, new TextElement("[OPTIONS]")));
            }
            else
            {
                CommandInfo? current = command;
                if (command.IsBranch)
                {
                    parameters.Push(new ColorElement(ConsoleColor.Cyan, new TextElement("<COMMAND>")));
                }

                while (current != null)
                {
                    var builder = new BlockElement();

                    if (!current.IsDefaultCommand)
                    {
                        builder.Append(new TextElement(current.Name));
                    }

                    if (current.Parameters.OfType<CommandArgument>().Any())
                    {
                        var isCurrent = current == command;
                        if (isCurrent)
                        {
                            foreach (var argument in current.Parameters.OfType<CommandArgument>()
                                .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                            {
                                var text = new TextElement($" <{argument.Value}>");
                                builder.Append(new ColorElement(ConsoleColor.Cyan, text));
                            }
                        }

                        var optionalArguments = current.Parameters.OfType<CommandArgument>().Where(x => !x.Required).ToArray();
                        if (optionalArguments.Length > 0 || !isCurrent)
                        {
                            foreach (var optionalArgument in optionalArguments)
                            {
                                var text = new TextElement($" [{optionalArgument.Value}]");
                                builder.Append(new ColorElement(ConsoleColor.Gray, text));
                            }
                        }
                    }

                    if (current == command)
                    {
                        builder.Append(new ColorElement(ConsoleColor.DarkGray, new TextElement(" [OPTIONS]")));
                    }

                    parameters.Push(builder);
                    current = current.Parent;
                }
            }

            composer.Color(ConsoleColor.Yellow, c => c.Text("USAGE:")).LineBreak();
            composer.Tab().Text(model.ApplicationName ?? Path.GetFileName(Assembly.GetEntryAssembly()?.Location));

            // Root or not a default command?
            if (command?.IsDefaultCommand != true)
            {
                composer.Space();
            }

            composer.Join(" ", parameters);
        }

        private static void WriteExamples(RenderableComposer composer, CommandModel model, CommandInfo? command)
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
                var prefix = model.ApplicationName ?? Path.GetFileName(Assembly.GetEntryAssembly()?.Location);

                composer.Color(ConsoleColor.Yellow, c => c.Text("EXAMPLES:")).LineBreak();
                for (int index = 0; index < Math.Min(maxExamples, examples.Count); index++)
                {
                    var args = string.Join(" ", examples[index]);

                    composer.Tab().Text(prefix);
                    composer.Space().Color(ConsoleColor.DarkGray, c => c.Text(args));
                    composer.LineBreak();
                }

                composer.LineBreak();
            }
        }

        private static void WriteArguments(RenderableComposer composer, CommandInfo? command)
        {
            var arguments = new List<HelpArgument>();
            arguments.AddRange(command?.Parameters?.OfType<CommandArgument>()?.Select(
                x => new HelpArgument(x.Value, x.Required, x.Description))
                ?? Array.Empty<HelpArgument>());

            if (arguments.Count == 0)
            {
                return;
            }

            composer.Color(ConsoleColor.Yellow, c => c.Text("ARGUMENTS:")).LineBreak();

            var maxLength = arguments.Max(arg => arg.Name.Length);

            foreach (var argument in arguments)
            {
                composer.Tab();

                // Argument name.
                composer.Condition(
                    argument.Required,
                    @true: c1 => c1.Color(ConsoleColor.Gray, c => c.Text($"<{argument.Name}>")),
                    @false: c1 => c1.Color(ConsoleColor.Gray, c => c.Text($"[{argument.Name}]")));

                // Description
                composer.Spaces(maxLength - argument.Name.Length);
                composer.Tab().Text(argument.Description?.TrimEnd('.')?.Trim());

                composer.LineBreak();
            }

            composer.LineBreak();
        }

        private static void WriteOptions(RenderableComposer composer, CommandInfo? command)
        {
            // Collect all options into a single structure.
            var parameters = new List<HelpOption>
            {
                new HelpOption("h", "help", null, null, "Prints help information"),
            };

            parameters.AddRange(command?.Parameters?.OfType<CommandOption>()?.Select(o =>
                new HelpOption(
                    o.ShortNames.FirstOrDefault(), o.LongNames.FirstOrDefault(),
                    o.ValueName, o.ValueIsOptional, o.Description))
                ?? Array.Empty<HelpOption>());

            var options = parameters.ToArray();
            if (options.Length > 0)
            {
                composer.Color(ConsoleColor.Yellow, c => c.Text("OPTIONS:")).LineBreak();

                // Start with composing a list of lines.
                var result = new List<Tuple<string?, BlockElement>>();
                foreach (var option in options)
                {
                    var item = new BlockElement();
                    item.Append(new TabElement());

                    // Short
                    if (option.Short != null)
                    {
                        item.Append(new TextElement($"-{option.Short}"));
                        if (option.Long != null)
                        {
                            item.Append(new TextElement(","));
                        }
                    }
                    else
                    {
                        item.Append(new RepeatingElement(3, new TextElement(" ")));
                    }

                    // Long
                    if (option.Long != null)
                    {
                        item.Append(new TextElement(" "));
                        item.Append(new TextElement($"--{option.Long}"));
                    }

                    // Value
                    if (option.Value != null)
                    {
                        item.Append(new TextElement(" "));

                        if (option.ValueIsOptional ?? false)
                        {
                            item.Append(new ColorElement(ConsoleColor.Gray, new TextElement($"[{option.Value}]")));
                        }
                        else
                        {
                            item.Append(new ColorElement(ConsoleColor.Gray, new TextElement($"<{option.Value}>")));
                        }
                    }

                    result.Add(Tuple.Create(option.Description, item));
                }

                // Now add the descriptions to all lines.
                var maxLength = result.Max(x => x.Item2.Length);
                foreach (var item in result)
                {
                    var description = item.Item1;
                    var element = item.Item2;

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        var neededSpaces = maxLength - element.Length;
                        if (neededSpaces > 0)
                        {
                            element.Append(new RepeatingElement(neededSpaces, new TextElement(" ")));
                        }

                        element.Append(new TabElement());
                        element.Append(new TextElement(description.TrimEnd('.').Trim()));
                    }

                    element.Append(new LineBreakElement());
                }

                // Append the items.
                composer.Append(result.Select(x => x.Item2));
                composer.LineBreak();
            }
        }

        private static void WriteCommands(
            RenderableComposer composer,
            CommandModel model,
            ICommandContainer command,
            bool isDefaultCommand)
        {
            var commands = isDefaultCommand ? model.Commands : command.Commands;
            commands = commands.Where(x => !x.IsHidden).ToList();

            if (commands.Count > 0)
            {
                composer.Color(ConsoleColor.Yellow, c => c.Text("COMMANDS:")).LineBreak();
                var maxCommandLength = commands.Max(x => x.Name.Length);
                foreach (var child in commands)
                {
                    composer.Tab().Text(child.Name);
                    composer.Spaces(maxCommandLength - child.Name.Length);
                    composer.Tab().Text(child.Description?.TrimEnd('.') ?? string.Empty);
                    composer.LineBreak();
                }
            }
        }
    }
}
