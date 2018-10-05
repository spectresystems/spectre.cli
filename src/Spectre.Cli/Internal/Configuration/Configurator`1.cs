using System;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class Configurator<TSettings> : IConfigurator<TSettings>
        where TSettings : CommandSettings
    {
        private readonly ConfiguredCommand _command;
        private readonly ITypeRegistrar _registrar;

        public Configurator(ConfiguredCommand command, ITypeRegistrar registrar)
        {
            _command = command;
            _registrar = registrar;
        }

        public void SetDescription(string description)
        {
            _command.Description = description;
        }

        public void AddCommand<TCommand>(string name) where TCommand : class, ICommandLimiter<TSettings>
        {
            var settingsType = ConfigurationHelper.GetSettingsType(typeof(TCommand));
            var command = new ConfiguredCommand(name, typeof(TCommand), settingsType);

            _command.Children.Add(command);
            _registrar.RegisterCommand(typeof(TCommand), settingsType);
        }

        public void AddBranch<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings
        {
            var command = new ConfiguredCommand(name, null, typeof(TDerivedSettings));
            action(new Configurator<TDerivedSettings>(command, _registrar));
            _command.Children.Add(command);
        }
    }
}
