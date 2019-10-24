using System;
using System.Collections.Generic;

namespace Spectre.Cli.Internal.Modelling
{
    internal sealed class CommandModel : ICommandContainer
    {
        public string? ApplicationName { get; }
        public ParsingMode ParsingMode { get; }
        public CommandInfo? DefaultCommand { get; }
        public IList<CommandInfo> Commands { get; }
        public IList<string[]> Examples { get; }

        public CommandModel(
            CommandAppSettings settings,
            CommandInfo? defaultCommand,
            IEnumerable<CommandInfo> commands,
            IEnumerable<string[]> examples)
        {
            ApplicationName = settings.ApplicationName;
            ParsingMode = settings.ParsingMode;
            DefaultCommand = defaultCommand;
            Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
            Examples = new List<string[]>(examples ?? Array.Empty<string[]>());
        }
    }
}
