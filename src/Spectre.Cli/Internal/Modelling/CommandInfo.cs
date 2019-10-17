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
        public HashSet<string> Aliases { get; }
        public string Description { get; }
        public object Data { get; }
        public Type CommandType { get; }
        public Type SettingsType { get; }
        public Func<CommandContext, CommandSettings, int> Delegate { get; }
        public bool IsDefaultCommand { get; }
        public CommandInfo Parent { get; }
        public IList<CommandInfo> Children { get; }
        public IList<CommandParameter> Parameters { get; }
        public IList<string[]> Examples { get; }

        public bool IsDelegate => Delegate != null;
        public bool IsBranch => CommandType == null && !IsDelegate;
        IList<CommandInfo> ICommandContainer.Commands => Children;

        public CommandInfo(CommandInfo parent, ConfiguredCommand prototype)
        {
            Parent = parent;

            Name = prototype.Name;
            Aliases = new HashSet<string>(prototype.Aliases);
            Description = prototype.Description;
            Data = prototype.Data;
            CommandType = prototype.CommandType;
            SettingsType = prototype.SettingsType;
            Delegate = prototype.Delegate;
            IsDefaultCommand = prototype.IsDefaultCommand;

            Children = new List<CommandInfo>();
            Parameters = new List<CommandParameter>();
            Examples = prototype.Examples ?? new List<string[]>();

            if (CommandType != null && string.IsNullOrWhiteSpace(Description))
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
