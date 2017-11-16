using System;
using System.Collections.Generic;

namespace Spectre.CommandLine.Internal.Configuration
{
    internal sealed class Configurator : IConfigurator, IConfiguration
    {
        public IList<ConfiguredCommand> Commands { get; }
        public string ApplicationName { get; private set; }

        public Configurator()
        {
            Commands = new List<ConfiguredCommand>();
        }

        public void SetApplicationName(string name)
        {
            ApplicationName = name;
        }

        public void AddCommand<TCommand>(string name) where TCommand : class, ICommand
        {
            var command = new ConfiguredCommand(name, typeof(TCommand), ConfigurationHelper.GetSettingsType(typeof(TCommand)));
            Commands.Add(command);
        }

        public void AddCommand<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : class
        {
            var command = new ConfiguredCommand(name, null, typeof(TSettings));
            action(new Configurator<TSettings>(command));
            Commands.Add(command);
        }
    }
}
