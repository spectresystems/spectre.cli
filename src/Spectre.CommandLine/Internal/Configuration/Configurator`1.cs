using System;

namespace Spectre.CommandLine.Internal.Configuration
{
    internal sealed class Configurator<TSettings> : IConfigurator<TSettings>
        where TSettings : class
    {
        private readonly ConfiguredCommand _command;

        public Configurator(ConfiguredCommand command)
        {
            _command = command;
        }

        public void SetDescription(string description)
        {
            _command.Description = description;
        }

        public void AddCommand<TCommand>(string name) where TCommand : class, ICommandLimiter<TSettings>
        {
            var command = new ConfiguredCommand(name, typeof(TCommand), ConfigurationHelper.GetSettingsType(typeof(TCommand)));
            _command.Children.Add(command);
        }

        public void AddCommand<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : class, TSettings
        {
            var command = new ConfiguredCommand(name, null, typeof(TDerivedSettings));
            action(new Configurator<TDerivedSettings>(command));
            _command.Children.Add(command);
        }
    }
}
