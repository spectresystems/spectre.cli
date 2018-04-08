using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Spectre.CommandLine.Internal.Modelling;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Internal.Rendering.Elements;

namespace Spectre.CommandLine.Internal
{
    internal static class HelpWriter
    {
        private class OptionInfo
        {
            public string Short { get; }
            public string Long { get; }
            public string Value { get; }
            public string Description { get; }

            public OptionInfo(string s, string l, string value, string description)
            {
                Short = s;
                Long = l;
                Value = value;
                Description = description;
            }

            public OptionInfo(CommandOption option)
            {
                Short = option.ShortName;
                Long = option.LongName;
                Value = option.ValueName;
                Description = option.Description;
            }
        }

        public static IRenderable Write(CommandModel model)
        {
            return WriteCommand(model, null);
        }

        public static IRenderable WriteCommand(CommandModel model, CommandInfo command)
        {
            var composer = new RenderableComposer();
            composer.LineBreak();

            // Usage
            WriteUsage(composer, model, command);
            composer.LineBreak().LineBreak();

            // Arguments
            WriteArguments(composer, command);

            // Options
            WriteOptions(composer, command);

            // Commands
            WriteCommands(composer, (ICommandContainer)command ?? model);

            composer.LineBreak();
            return composer;
        }

        private static void WriteUsage(RenderableComposer composer, CommandModel model, CommandInfo command)
        {
            var stack = new Stack<IRenderable>();

            if (command == null)
            {
                stack.Push(new ColorElement(ConsoleColor.Cyan, new TextElement("<COMMAND>")));
                stack.Push(new ColorElement(ConsoleColor.DarkGray, new TextElement("[OPTIONS]")));
            }
            else
            {
                var current = command;
                if (command.IsProxy)
                {
                    stack.Push(new ColorElement(ConsoleColor.Cyan, new TextElement("<COMMAND>")));
                }

                while (current != null)
                {
                    var builder = new BlockElement();
                    builder.Append(new TextElement(current.Name));

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
                                builder.Append(new ColorElement(ConsoleColor.DarkGray, text));
                            }
                        }
                    }

                    builder.Append(new ColorElement(ConsoleColor.DarkGray, new TextElement(" [OPTIONS]")));

                    stack.Push(builder);
                    current = current.Parent;
                }
            }

            composer.Color(ConsoleColor.Yellow, c => c.Text("USAGE:")).LineBreak();
            composer.Tab().Text(model.ApplicationName ?? Assembly.GetEntryAssembly().GetName().Name);
            composer.Space().Join(" ", stack);
        }

        private static void WriteOptions(RenderableComposer composer, CommandInfo command)
        {
            // Collect all options into a single structure.
            var parameters = new List<OptionInfo>();
            parameters.Add(new OptionInfo("h", "help", null, "Prints help information"));
            parameters.AddRange(command?.Parameters?.OfType<CommandOption>()?.Select(o => new OptionInfo(o)) ?? Array.Empty<OptionInfo>());

            var options = parameters.ToArray();
            if (options.Length > 0)
            {
                composer.Color(ConsoleColor.Yellow, c => c.Text("OPTIONS:")).LineBreak();

                // Start with composing a list of lines.
                var result = new List<(string description, BlockElement element)>();
                foreach (var option in options)
                {
                    // Short
                    var item = new BlockElement();
                    item.Append(new TabElement());

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
                        item.Append(new ColorElement(ConsoleColor.DarkGray, new TextElement($"<{option.Value}>")));
                    }

                    result.Add((option.Description, item));
                }

                // Now add the descriptions to all lines.
                var maxLength = result.Max(x => x.element.Length);
                foreach (var (description, element) in result)
                {
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
                composer.Append(result.Select(x => x.element));
                composer.LineBreak();
            }
        }

        private static void WriteArguments(RenderableComposer composer, CommandInfo command)
        {
            var arguments = new List<(string name, bool required, string description)>();
            arguments.AddRange(command?.Parameters?.OfType<CommandArgument>()?.Select(
                x => (x.Value, x.Required, x.Description))
                ?? Array.Empty<(string, bool, string)>());

            if (arguments.Count == 0)
            {
                return;
            }

            composer.Color(ConsoleColor.Yellow, c => c.Text("ARGUMENTS:")).LineBreak();

            var maxArgLength = arguments.Max(x => x.name.Length);
            foreach (var (name, required, description) in arguments)
            {
                composer.Tab();

                composer.Condition(required,
                    @true: c1 => c1.Color(ConsoleColor.DarkGray, c => c.Text($"<{name}>")),
                    @false: c1 => c1.Color(ConsoleColor.DarkGray, c => c.Text($"[{name}]")));

                composer.Spaces(maxArgLength - name.Length);
                composer.Tab().Text(description?.TrimEnd('.')?.Trim());

                composer.LineBreak();
            }

            composer.LineBreak();
        }

        private static void WriteCommands(RenderableComposer composer, ICommandContainer command)
        {
            if (command.Commands.Count > 0)
            {
                composer.Color(ConsoleColor.Yellow, c => c.Text("COMMANDS:")).LineBreak();
                var maxCommandLength = command.Commands.Max(x => x.Name.Length);
                foreach (var child in command.Commands)
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
