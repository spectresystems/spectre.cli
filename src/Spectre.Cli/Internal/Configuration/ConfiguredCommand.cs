using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class ConfiguredCommand
    {
        public string Name { get; }
        public string Description { get; set; }
        public Type CommandType { get; }
        public Type SettingsType { get; }
        public bool IsDefaultCommand { get; }

        public IList<ConfiguredCommand> Children { get; }
        public IList<string[]> Examples { get; }

        public ConfiguredCommand(string name, Type commandType, Type settingsType, bool defaultCommand = false)
        {
            Name = name;
            CommandType = commandType;
            SettingsType = settingsType;
            IsDefaultCommand = defaultCommand;
            Children = new List<ConfiguredCommand>();
            Examples = new List<string[]>();
        }
    }
}
