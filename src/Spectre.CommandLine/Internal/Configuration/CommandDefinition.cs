using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class CommandDefinition : ICommandContainer
    {
        public string Name { get; }
        public Type CommandType { get; }
        public Type SettingsType { get; set; }
        public CommandDefinition Parent { get; }
        public ICollection<OptionDefinition> Options { get; }
        public ICollection<CommandDefinition> Commands { get; set; }

        public bool IsProxy => CommandType == null;

        public CommandDefinition(CommandDefinition parent, string name, Type commandType, Type settingsType)
        {
            Parent = parent;
            Name = name;
            CommandType = commandType;
            SettingsType = settingsType;
            Options = new List<OptionDefinition>();
            Commands = new List<CommandDefinition>();
        }
    }
}