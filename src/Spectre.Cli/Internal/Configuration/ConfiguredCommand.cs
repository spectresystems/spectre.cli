using System;
using System.Collections.Generic;
using Spectre.Cli.Internal.Exceptions;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class ConfiguredCommand
    {
        public string Name { get; }
        public HashSet<string> Aliases { get; }
        public string? Description { get; set; }
        public object? Data { get; set; }
        public Type? CommandType { get; }
        public Type SettingsType { get; }
        public Func<CommandContext, CommandSettings, int>? Delegate { get; }
        public bool IsDefaultCommand { get; }

        public IList<ConfiguredCommand> Children { get; }
        public IList<string[]> Examples { get; }

        private ConfiguredCommand(
            string name,
            Type? commandType,
            Type settingsType,
            Func<CommandContext, CommandSettings, int>? @delegate,
            bool isDefaultCommand)
        {
            Name = name;
            Aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CommandType = commandType;
            SettingsType = settingsType;
            Delegate = @delegate;
            IsDefaultCommand = isDefaultCommand;

            Children = new List<ConfiguredCommand>();
            Examples = new List<string[]>();
        }

        public static ConfiguredCommand FromBranch<TSettings>(string name)
            where TSettings : CommandSettings
        {
            return new ConfiguredCommand(name, null, typeof(TSettings), null, false);
        }

        public static ConfiguredCommand FromType<TCommand>(string name, bool isDefaultCommand = false)
            where TCommand : class, ICommand
        {
            var settingsType = ConfigurationHelper.GetSettingsType(typeof(TCommand));
            if (settingsType == null)
            {
                throw RuntimeException.CouldNotGetSettingsType(typeof(TCommand));
            }
            return new ConfiguredCommand(name, typeof(TCommand), settingsType, null, isDefaultCommand);
        }

        public static ConfiguredCommand FromDelegate<TSettings>(
            string name, Func<CommandContext, CommandSettings, int>? @delegate = null)
                where TSettings : CommandSettings
        {
            return new ConfiguredCommand(name, null, typeof(TSettings), @delegate, false);
        }
    }
}
