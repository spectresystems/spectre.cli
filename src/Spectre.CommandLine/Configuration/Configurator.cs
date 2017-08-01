using System;
using Spectre.CommandLine.Annotations;

namespace Spectre.CommandLine.Configuration
{
    internal sealed class Configurator : IConfigurator
    {
        public Configuration Configuration { get; }

        public Configurator()
        {
            Configuration = new Configuration();
        }

        public void SetApplicationName(string name)
        {
            Configuration.ApplicationName = name;
        }

        public void SetHelpOption(string template)
        {
            Configuration.Help = new OptionAttribute(template);
        }

        public void AddProxy<TSettings>(string name, Action<IConfigurator<TSettings>> action)
        {
            var command = CommandInfoFactory.CreateProxy(null, name, typeof(TSettings));
            action(new Configurator<TSettings>(command));
            Configuration.Commands.Add(command);
        }

        public void AddCommand<TCommand>(string name)
            where TCommand : ICommand
        {
            var command = CommandInfoFactory.CreateCommand(null, name, typeof(TCommand));
            Configuration.Commands.Add(command);
        }
    }
}