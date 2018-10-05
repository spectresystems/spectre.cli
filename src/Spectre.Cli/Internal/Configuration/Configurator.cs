using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class Configurator : IConfigurator, IConfiguration
    {
        private readonly ITypeRegistrar _registrar;

        public IList<ConfiguredCommand> Commands { get; }
        public ConfiguredCommand DefaultCommand { get; }
        public string ApplicationName { get; private set; }
        public bool ShouldPropagateExceptions { get; private set; }
        public ParsingMode ParsingMode { get; private set; }

        public Configurator(ITypeRegistrar registrar, Type defaultCommand = null)
        {
            _registrar = registrar;

            Commands = new List<ConfiguredCommand>();
            ShouldPropagateExceptions = false;
            ParsingMode = ParsingMode.Relaxed;

            if (defaultCommand != null)
            {
                if (!typeof(ICommand).IsAssignableFrom(defaultCommand))
                {
                    throw new ArgumentException($"The specified default command type '{defaultCommand}' is not a command.", nameof(defaultCommand));
                }

                // Initialize the default command.
                var settingsType = ConfigurationHelper.GetSettingsType(defaultCommand);
                DefaultCommand = new ConfiguredCommand(Constants.DefaultCommandName, defaultCommand, settingsType, true);

                // Register the default command.
                _registrar.RegisterCommand(defaultCommand, settingsType);
            }
        }

        public void SetApplicationName(string name)
        {
            ApplicationName = name;
        }

        public void UseStrictParsing()
        {
            ParsingMode = ParsingMode.Strict;
        }

        public void PropagateExceptions()
        {
            ShouldPropagateExceptions = true;
        }

        public void AddCommand<TCommand>(string name) where TCommand : class, ICommand
        {
            var settingsType = ConfigurationHelper.GetSettingsType(typeof(TCommand));
            var command = new ConfiguredCommand(name, typeof(TCommand), settingsType);
            Commands.Add(command);

            _registrar.RegisterCommand(typeof(TCommand), settingsType);
        }

        public void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings
        {
            var command = new ConfiguredCommand(name, null, typeof(TSettings));
            action(new Configurator<TSettings>(command, _registrar));
            Commands.Add(command);
        }
    }
}
