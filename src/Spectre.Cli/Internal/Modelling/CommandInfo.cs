using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Spectre.Cli.Internal.Configuration;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandInfo : ICommandContainer
    {
        public string Name { get; }
        public string Description { get; }
        public Type CommandType { get; }
        public Type SettingsType { get; }
        public bool IsDefaultCommand { get; }
        public CommandInfo Parent { get; }
        public IList<CommandInfo> Children { get; }
        public IList<CommandParameter> Parameters { get; }
        public IList<string[]> Examples { get; }

        public bool IsBranch => CommandType == null;
        IList<CommandInfo> ICommandContainer.Commands => Children;

        public CommandInfo(CommandInfo parent, ConfiguredCommand prototype)
        {
            Parent = parent;

            Name = prototype.Name;
            Description = prototype.Description;
            CommandType = prototype.CommandType;
            SettingsType = prototype.SettingsType;
            IsDefaultCommand = prototype.IsDefaultCommand;

            Children = new List<CommandInfo>();
            Parameters = new List<CommandParameter>();
            Examples = prototype.Examples ?? new List<string[]>();

            if (!IsBranch)
            {
                var description = CommandType.GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    Description = description.Description;
                }
            }
        }
    }
}
