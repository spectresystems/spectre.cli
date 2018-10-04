using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandModel : ICommandContainer
    {
        public string ApplicationName { get; }
        public ParsingMode ParsingMode { get; }
        public CommandInfo DefaultCommand { get; }
        public IList<CommandInfo> Commands { get; }

        public CommandModel(string applicationName, ParsingMode parsingMode, CommandInfo defaultCommand, IEnumerable<CommandInfo> commands)
        {
            ApplicationName = applicationName;
            ParsingMode = parsingMode;
            DefaultCommand = defaultCommand;
            Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
        }
    }
}
