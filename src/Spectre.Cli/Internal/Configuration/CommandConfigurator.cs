using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.Cli.Internal.Configuration
{
    internal sealed class CommandConfigurator : ICommandConfigurator
    {
        public ConfiguredCommand Command { get; }

        public CommandConfigurator(ConfiguredCommand command)
        {
            Command = command;
        }

        public ICommandConfigurator WithExample(string[] args)
        {
            Command.Examples.Add(args);
            return this;
        }

        public ICommandConfigurator WithAlias(string alias)
        {
            Command.Aliases.Add(alias);
            return this;
        }
    }
}
