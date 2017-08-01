using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.CommandLine.Configuration;

namespace Spectre.CommandLine.Utilities
{
    internal static class HelpPrinter
    {
        public static void Write(IConfiguration configuration)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {configuration.ApplicationName} [command]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            foreach (var command in configuration.Commands)
            {
                Console.WriteLine($"  {BuildCommandString(command)}");
            }
            Console.WriteLine();
        }

        public static void Write(CommandInfo command, IConfiguration configuration)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {configuration.ApplicationName} {BuildUsageString(command)}");

            var options = command.Parameters.OfType<CommandOption>().ToArray();
            if (options.Length > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Options for {command.Name}:");
                foreach (var option in options.Where(x => !x.Info.IsInherited))
                {
                    Console.WriteLine($"  {BuildOptionString(option)}");
                }
            }

            if (command.IsProxy)
            {
                Console.WriteLine();
                Console.WriteLine("Commands:");
                foreach (var childCommand in command.Commands)
                {
                    Console.WriteLine($"  {BuildCommandString(childCommand)}");
                }
            }

            Console.WriteLine();
        }

        private static string BuildCommandString(CommandInfo command)
        {
            return $"{command.Name}  {command.Description}".Trim();
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
                builder.Add("<" + option.ValueName + ">");
            }
            if (!string.IsNullOrWhiteSpace(option.Info.Description))
            {
                builder.Add(" " + option.Info.Description);
            }
            if (option.Info.IsRequired)
            {
                builder.Add(" [Required]");
            }
            return string.Join(" ", builder);
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
