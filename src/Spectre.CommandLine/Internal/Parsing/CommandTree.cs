using System;
using System.Collections.Generic;
using Spectre.CommandLine.Internal.Modelling;

namespace Spectre.CommandLine.Internal.Parsing
{
    internal sealed class CommandTree
    {
        public CommandInfo Command { get; }
        public List<(CommandParameter, string)> Mapped { get; }
        public List<CommandParameter> Unmapped { get; }
        public CommandTree Parent { get; }
        public CommandTree Next { get; set; }
        public bool ShowHelp { get; set; }

        public CommandTree(CommandTree parent, CommandInfo command)
        {
            Parent = parent;
            Command = command;
            Mapped = new List<(CommandParameter, string)>();
            Unmapped = new List<CommandParameter>();
        }

        public object CreateSettings(ITypeResolver resolver)
        {
            var settings = resolver.Activate(Command.SettingsType);
            if (settings != null)
            {
                return settings;
            }
            throw new InvalidOperationException($"Could not create settings of type '{Command.SettingsType.FullName}'.");
        }

        public ICommand CreateCommand(ITypeResolver resolver)
        {
            if (resolver.Activate(Command.CommandType) is ICommand command)
            {
                return command;
            }
            throw new InvalidOperationException($"Could not create command of type '{Command.CommandType.FullName}'.");
        }
    }
}