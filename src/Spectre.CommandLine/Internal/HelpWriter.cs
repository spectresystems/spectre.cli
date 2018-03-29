using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.CommandLine.Internal.Modelling;

namespace Spectre.CommandLine.Internal
{
    internal static class HelpWriter
    {
        public static void Write(CommandModel model)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {GetApplicationName(model)} [command]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            foreach (var command in model.Commands)
            {
                Console.WriteLine($"  {BuildCommandString(command)}");
            }
            Console.WriteLine();
        }

        public static void Write(CommandModel model, CommandInfo command)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {GetApplicationName(model)} {BuildUsageString(command)}");

            var arguments = command.Parameters.OfType<CommandArgument>().ToArray();
            if (arguments.Length > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Arguments:");
                foreach (var argument in arguments.OrderBy(a => a.Position))
                {
                    Console.WriteLine($"  {BuildArgumentString(argument)}");
                }
            }

            var options = command.Parameters.OfType<CommandOption>().ToArray();
            if (options.Length > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Options:");
                foreach (var option in options)
                {
                    Console.WriteLine($"  {BuildOptionString(option)}");
                }
            }

            if (command.IsProxy)
            {
                Console.WriteLine();
                Console.WriteLine("Commands:");
                foreach (var childCommand in command.Children)
                {
                    Console.WriteLine($"  {BuildCommandString(childCommand)}");
                }
            }

            Console.WriteLine();
        }

        private static string GetApplicationName(CommandModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.ApplicationName))
            {
                return model.ApplicationName;
            }
            return System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }

        private static string BuildArgumentString(CommandArgument argument)
        {
            var builder = new List<string>();
            if (argument.Required)
            {
                builder.Add("<" + argument.Value + ">");
            }
            else
            {
                builder.Add("[" + argument.Value + "]");
            }
            if (!string.IsNullOrWhiteSpace(argument.Description))
            {
                builder.Add(" " + argument.Description);
            }
            return string.Join(" ", builder);
        }

        private static string BuildCommandString(CommandInfo command)
        {
            return $"{command.Name}        {command.Description}".Trim();
        }

        private static string BuildOptionString(CommandOption option)
        {
            var builder = new List<string>();
            if (!string.IsNullOrWhiteSpace(option.ShortName))
            {
                builder.Add("-" + option.ShortName);
            }
            if (!string.IsNullOrWhiteSpace(option.LongName))
            {
                if (builder.Count > 0)
                {
                    builder.Add("|");
                }
                builder.Add("--" + option.LongName);
            }
            if (!string.IsNullOrWhiteSpace(option.ValueName))
            {
                if (option.Required)
                {
                    builder.Add(" <" + option.ValueName + ">");
                }
                else
                {
                    builder.Add(" [" + option.ValueName + "]");
                }
            }
            if (!string.IsNullOrWhiteSpace(option.Description))
            {
                builder.Add("    " + option.Description);
            }
            return string.Concat(builder);
        }

        private static string BuildUsageString(CommandInfo command)
        {
            var current = command;
            var stack = new Stack<string>();
            if (command.IsProxy)
            {
                stack.Push("[command]");
            }
            while (current != null)
            {
                var builder = new StringBuilder();
                builder.Append(current.Name);
                if (current.Parameters.OfType<CommandArgument>().Any())
                {
                    var isCurrent = current == command;
                    if (isCurrent)
                    {
                        foreach (var argument in current.Parameters.OfType<CommandArgument>()
                            .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                        {
                            builder.Append(" <").Append(argument.Value).Append(">");
                        }
                    }

                    var optionalArguments = current.Parameters.OfType<CommandArgument>().Where(x => !x.Required).ToArray();
                    if (optionalArguments.Length > 0 || !isCurrent)
                    {
                        builder.Append(" [arguments]");
                    }
                }
                if (current.Parameters.OfType<CommandOption>().Any())
                {
                    builder.Append(" [options]");
                }
                stack.Push(builder.ToString());
                current = current.Parent;
            }
            return string.Join(" ", stack);
        }
    }
}
