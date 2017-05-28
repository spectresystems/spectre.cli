using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class Configurator : IConfigurator, ICommandContainer
    {
        private readonly List<CommandDefinition> _commands;

        public ICollection<CommandDefinition> Commands => _commands;

        public Configurator()
        {
            _commands = new List<CommandDefinition>();
        }

        public void AddProxy<TSettings>(string name, Action<IConfigurator<TSettings>> action)
        {
            var command = CommandDefinitionFactory.CreateProxy(null, name, typeof(TSettings));
            action(new Configurator<TSettings>(command));
            Commands.Add(command);
        }

        public void AddCommand<TCommand>(string name)
            where TCommand : ICommand
        {
            var command = CommandDefinitionFactory.CreateCommand(null, name, typeof(TCommand));
            Commands.Add(command);
        }
    }

    internal sealed class Configurator<TSettings> : IConfigurator<TSettings>
    {
        private readonly CommandDefinition _parent;

        public Configurator(CommandDefinition parent)
        {
            _parent = parent;
        }

        public void AddProxy<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings
        {
            var proxy = CommandDefinitionFactory.CreateProxy(_parent, name, typeof(TDerivedSettings));
            action(new Configurator<TDerivedSettings>(proxy));
            _parent.Commands.Add(proxy);
        }

        public void AddCommand<TCommand>(string name)
            where TCommand : ICommand, ICommandLimiter<TSettings>
        {
            var definition = CommandDefinitionFactory.CreateCommand(_parent, name, typeof(TCommand));
            _parent.Commands.Add(definition);
        }
    }
}