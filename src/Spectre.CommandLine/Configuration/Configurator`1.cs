using System;

namespace Spectre.CommandLine.Configuration
{
    internal sealed class Configurator<TSettings> : IConfigurator<TSettings>
    {
        private readonly CommandInfo _parent;

        public Configurator(CommandInfo parent)
        {
            _parent = parent;
        }

        public void SetDescription(string description)
        {
            _parent.Description = description;
        }

        public void AddProxy<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings
        {
            var proxy = CommandInfoFactory.CreateProxy(_parent, name, typeof(TDerivedSettings));
            action(new Configurator<TDerivedSettings>(proxy));
            _parent.Commands.Add(proxy);
        }

        public void AddCommand<TCommand>(string name)
            where TCommand : ICommand, ICommandLimiter<TSettings>
        {
            var command = CommandInfoFactory.CreateCommand(_parent, name, typeof(TCommand));
            _parent.Commands.Add(command);
        }
    }
}
