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
            composer.LineBreak();

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
            composer.Color(ConsoleColor.Yellow, c => c.Text("OPTIONS:")).LineBreak();

            // Collect all options into a single structure.
            var options = new List<(string @short, string @long, string @description)>();
            options.Add(("h", "help", "Prints help information"));
            options.AddRange(command?.Parameters?.OfType<CommandOption>()?.Select(
                x => (x.ShortName, x.LongName, x.Description))
                ?? Array.Empty<(string, string, string)>());

            var maxLongLength = options.Max(x => x.@long?.Length ?? 0);
            foreach (var (@short, @long, description) in options)
            {
                composer.Tab();

                composer.Condition(@short != null,
                    @true: c1 => c1.Condition(@long != null,
                        @true: c2 => c2.Text($"-{@short},"),
                        @false: c2 => c2.Text($"-{@short} ")),
                    @false: c => c.Text("   "));

                composer.Space();
                composer.Condition(@long != null,
                    @true: c1 => c1.Text($"--{@long}"),
                    @false: c1 => c1.Spaces(maxLongLength + 2));

                composer.Spaces(maxLongLength - @long?.Length ?? 0);
                composer.Tab().Text(description?.TrimEnd('.'));

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
                    @true: c1 => c1.Text($"<{name}>"),
                    @false: c1 => c1.Text($"[{name}]"));

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
