using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandModel : ICommandContainer
    {
        public string ApplicationName { get; }
        public CommandInfo DefaultCommand { get; }
        public IList<CommandInfo> Commands { get; }

        public CommandModel(string applicationName, CommandInfo defaultCommand, IEnumerable<CommandInfo> commands)
        {
            ApplicationName = applicationName;
            DefaultCommand = defaultCommand;
            Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
        }
    }
}
