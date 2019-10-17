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

        public void AddExample(string[] args)
        {
            _command.Examples.Add(args);
        }

        public ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommandLimiter<TSettings>
        {
            var command = ConfiguredCommand.FromType<TCommand>(name);
            var configurator = new CommandConfigurator(command);

            _command.Children.Add(command);
            _registrar.RegisterCommand(command);

            return configurator;
        }

        public ICommandConfigurator AddDelegate<TDerivedSettings>(string name, Func<CommandContext, TDerivedSettings, int> func)
            where TDerivedSettings : TSettings
        {
            var command = ConfiguredCommand.FromDelegate<TDerivedSettings>(
                name, (context, settings) => func(context, (TDerivedSettings)settings));

            _command.Children.Add(command);
            var configurator = new CommandConfigurator(command);

            return configurator;
        }

        public void AddBranch<TDerivedSettings>(string name, Action<IConfigurator<TDerivedSettings>> action)
            where TDerivedSettings : TSettings
        {
            var command = ConfiguredCommand.FromBranch<TDerivedSettings>(name);
            action(new Configurator<TDerivedSettings>(command, _registrar));
            _command.Children.Add(command);
        }
    }
}
