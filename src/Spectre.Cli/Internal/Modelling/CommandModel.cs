using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandModel : ICommandContainer
    {
        public string ApplicationName { get; }
        public IList<CommandInfo> Commands { get; }

        public CommandModel(string applicationName, IEnumerable<CommandInfo> commands)
        {
            ApplicationName = applicationName;
            Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
        }
    }
}
