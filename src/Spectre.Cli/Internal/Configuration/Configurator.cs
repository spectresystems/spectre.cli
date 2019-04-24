using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class Configurator : IConfigurator, IConfiguration
    {
        private readonly ITypeRegistrar _registrar;

        public IList<ConfiguredCommand> Commands { get; }
        public ConfigurationSettings Settings { get; }
        public ConfiguredCommand DefaultCommand { get; private set; }
        public IList<string[]> Examples { get; }

        public Configurator(ITypeRegistrar registrar)
        {
            _registrar = registrar;

            Commands = new List<ConfiguredCommand>();
            Settings = new ConfigurationSettings();
            Examples = new List<string[]>();
        }

        public void SetApplicationName(string name)
        {
            Settings.ApplicationName = name;
        }

        public void UseStrictParsing()
        {
            Settings.ParsingMode = ParsingMode.Strict;
        }

        public void PropagateExceptions()
        {
            Settings.PropagateExceptions = true;
        }

        public void ValidateExamples()
        {
            Settings.ValidateExamples = true;
        }

        public void AddExample(string[] args)
        {
            Examples.Add(args);
        }

        public void SetDefaultCommand<TDefaultCommand>()
            where TDefaultCommand : class, ICommand
        {
            // Get the type.
            var defaultCommand = typeof(TDefaultCommand);

            // Initialize the default command.
            var settingsType = ConfigurationHelper.GetSettingsType(defaultCommand);
            DefaultCommand = new ConfiguredCommand(Constants.DefaultCommandName, defaultCommand, settingsType, true);

            // Register the default command.
            _registrar.RegisterCommand(defaultCommand, settingsType);
        }

        public ICommandConfigurator AddCommand<TCommand>(string name) where TCommand : class, ICommand
        {
            var settingsType = ConfigurationHelper.GetSettingsType(typeof(TCommand));
            var command = new ConfiguredCommand(name, typeof(TCommand), settingsType);
            var configurator = new CommandConfigurator(command);

            Commands.Add(command);
            _registrar.RegisterCommand(typeof(TCommand), settingsType);

            return configurator;
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
