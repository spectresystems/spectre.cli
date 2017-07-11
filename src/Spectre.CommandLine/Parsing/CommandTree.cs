using System.Collections.Generic;
using Spectre.CommandLine.Configuration;

namespace Spectre.CommandLine.Parsing
{
    internal sealed class CommandTree
    {
        public CommandInfo Command { get; }
        public List<(CommandParameter, string)> Mapped { get; }
        public List<CommandParameter> Unmapped { get; }
        public CommandTree Parent { get; }
        public CommandTree Next { get; set; }

        public bool ShowHelp { get; set; }

        public CommandTree(CommandTree parent, CommandInfo command)
        {
            Parent = parent;
            Command = command;
            Mapped = new List<(CommandParameter, string)>();
            Unmapped = new List<CommandParameter>();
        }

        public CommandTree GetBottomCommand()
        {
            var node = this;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        public CommandTree GetTopCommand()
        {
            var node = this;
            while (node.Next != null)
            {
                node = node.Next;
            }
            return node;
        }
    }
}
