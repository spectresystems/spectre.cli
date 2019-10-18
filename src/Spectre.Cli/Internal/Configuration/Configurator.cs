using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class Configurator : IConfigurator, IConfiguration
    {
        private readonly ITypeRegistrar? _registrar;

        public IList<ConfiguredCommand> Commands { get; }
        public ConfigurationSettings Settings { get; }
        public ConfiguredCommand? DefaultCommand { get; private set; }
        public IList<string[]> Examples { get; }

        public Configurator(ITypeRegistrar? registrar)
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
            DefaultCommand = ConfiguredCommand.FromType<TDefaultCommand>(
                Constants.DefaultCommandName, isDefaultCommand: true);

            _registrar?.RegisterCommand(DefaultCommand);
        }

        public ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommand
        {
            var command = Commands.AddAndReturn(ConfiguredCommand.FromType<TCommand>(name, false));
            _registrar?.RegisterCommand(command);
            return new CommandConfigurator(command);
        }

        public ICommandConfigurator AddDelegate<TSettings>(string name, Func<CommandContext, TSettings, int> func)
            where TSettings : CommandSettings
        {
            var command = Commands.AddAndReturn(ConfiguredCommand.FromDelegate<TSettings>(
                name, (context, settings) => func(context, (TSettings)settings)));

            return new CommandConfigurator(command);
        }

        public void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings
        {
            var command = ConfiguredCommand.FromBranch<TSettings>(name);
            action(new Configurator<TSettings>(command, _registrar));
            Commands.Add(command);
        }
    }
}
